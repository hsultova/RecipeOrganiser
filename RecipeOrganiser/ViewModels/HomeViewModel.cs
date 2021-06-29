using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using RecipeOrganiser.Data.Models;
using RecipeOrganiser.Data.Repositories;
using RecipeOrganiser.Utils;
using RecipeOrganiser.Utils.Events;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class HomeViewModel : BaseViewModel
	{
		private readonly IMapper _mapper;

		private readonly RecipeViewModel _recipeViewModel;
		private readonly ShoppingListViewModel _shoppingListViewModel;

		private readonly IRecipeRepository _recipeRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IIngredientRepository _ingredientRepository;
		private readonly IShoppingListRepository _shoppingListRepository;

		public HomeViewModel(
			IMapper mapper,
			IRecipeRepository recipeRepository,
			ICategoryRepository categoryRepository,
			IIngredientRepository ingredientRepository,
			IShoppingListRepository shoppingListRepository,
			RecipeViewModel recipeViewModel,
			ShoppingListViewModel shoppingListViewModel)
		{
			_mapper = mapper;

			_recipeRepository = recipeRepository;
			_categoryRepository = categoryRepository;
			_ingredientRepository = ingredientRepository;
			_shoppingListRepository = shoppingListRepository;

			_recipeViewModel = recipeViewModel;
			_recipeViewModel.Title = "Edit Recipe";

			_shoppingListViewModel = shoppingListViewModel;

			RecipesView = CollectionViewSource.GetDefaultView(Recipes);
			RecipesView.Filter = Filter;
		}

		private ObservableCollection<Recipe> _recipes;
		public ObservableCollection<Recipe> Recipes
		{
			get
			{
				if (_recipes == null)
				{
					_recipes = new ObservableCollection<Recipe>(_recipeRepository.GetAll());
				}
				return _recipes;
			}
		}

		public List<Recipe> SelectedRecipes { get; set; } = new List<Recipe>();

		public ICollectionView RecipesView { get; set; }

		private string _selectedCategory;
		public string SelectedCategory
		{
			get
			{
				return _selectedCategory;
			}
			set
			{
				if (SetBackingFieldProperty<string>(ref _selectedCategory, value, nameof(SelectedCategory)))
				{
					RecipesView.Refresh();
				}
			}
		}

		private List<string> _categories;
		public List<string> Categories
		{
			get
			{
				if (_categories == null)
				{
					_categories = _categoryRepository.GetAll().Select(c => c.Name).ToList();
				}
				return _categories;
			}
			set
			{
				SetBackingFieldProperty<List<string>>(ref _categories, value, nameof(Categories));
			}
		}

		private string _selectedIngredient;
		public string SelectedIngredient
		{
			get
			{
				return _selectedIngredient;
			}
			set
			{
				if (SetBackingFieldProperty<string>(ref _selectedIngredient, value, nameof(SelectedIngredient)))
				{
					RecipesView.Refresh();
				}
			}
		}

		private List<string> _ingredients;
		public List<string> Ingredients
		{
			get
			{
				if (_ingredients == null)
				{
					_ingredients = _ingredientRepository.GetAll().Select(i => i.Name).ToList();
				}
				return _ingredients;
			}
			set
			{
				SetBackingFieldProperty<List<string>>(ref _ingredients, value, nameof(Ingredients));
			}
		}

		private ObservableCollection<ShoppingList> _shoppingLists;
		public ObservableCollection<ShoppingList> ShoppingLists
		{
			get
			{
				if (_shoppingLists == null)
				{
					_shoppingLists = new ObservableCollection<ShoppingList>(_shoppingListRepository.GetAll());
				}
				return _shoppingLists;
			}
		}

		private string _searchText;
		public string SearchText
		{
			get
			{
				return _searchText;
			}
			set
			{
				SetBackingFieldProperty<string>(ref _searchText, value, nameof(SearchText));
			}
		}

		private bool _isAdvancedFilterEnabled = false;
		public bool IsFilterEnabled
		{
			get
			{
				return _isAdvancedFilterEnabled;
			}
			set
			{
				SetBackingFieldProperty<bool>(ref _isAdvancedFilterEnabled, value, nameof(IsFilterEnabled));
			}
		}

		#region Commands
		public ICommand SearchCommand => new RelayCommand(Search);
		public ICommand EditCommand => new RelayCommand(Edit);
		public ICommand DeleteCommand => new RelayCommand(Delete);
		public ICommand AddNewShoppingListCommand => new RelayCommand(AddNewShoppingList);
		public ICommand AddToShoppingListCommand => new RelayCommand(AddToShoppingList);

		public ICommand DoubleClickCommand => new RelayCommand(DoubleClick);

		#endregion

		private void Search(object obj)
		{
			RecipesView.Refresh();
		}

		private void Edit(object obj)
		{
			var recipe = _recipeRepository.Get(r => r.Id == ((Recipe) obj).Id, r => r.RecipeIngredients);

			_mapper.Map(recipe, _recipeViewModel);
			_recipeViewModel.CurrentRecipe = recipe;
			_recipeViewModel.CategoryName = recipe.Category.Name;
			OnChangeViewModel(new ChangeViewModelEventArgs { ViewModel = _recipeViewModel });
		}

		private void Delete(object obj)
		{
			foreach (Recipe selectedRecipe in SelectedRecipes)
			{
				var result = MessageBox.Show($"Are you sure you want to delete '{selectedRecipe.Name}' recipe?", "Confirm", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.No)
				{
					continue;
				}
				_recipeRepository.Delete(selectedRecipe.Id);
				OnRecordDeleted<Recipe>(selectedRecipe.Name);
			}

			_recipeRepository.SaveChanges();
			SelectedRecipes.Clear();
			Refresh();
		}

		private void AddNewShoppingList(object obj)
		{
			var shoppingList = new ShoppingList { Name = "Untitled" };

			shoppingList.ShoppingListRecipes = new List<ShoppingListRecipe>();
			shoppingList.ShoppingListIngredients = new List<ShoppingListIngredient>();

			foreach (var recipe in SelectedRecipes)
			{
				_recipeRepository.Get(x => x.Id == recipe.Id, x => x.RecipeIngredients);
				var shoppingListRecipe = new ShoppingListRecipe
				{
					Recipe = recipe,
					ShoppingList = shoppingList
				};
				shoppingList.ShoppingListRecipes.Add(shoppingListRecipe);

				foreach (var recipeIngredient in recipe.RecipeIngredients)
				{
					var shoppingListIngredient = new ShoppingListIngredient();
					_mapper.Map(recipeIngredient, shoppingListIngredient, nameof(recipeIngredient.Id));

					shoppingList.ShoppingListIngredients.Add(shoppingListIngredient);
				}
			}

			_shoppingListRepository.Create(shoppingList);
			_shoppingListRepository.SaveChanges();

			_shoppingListViewModel.SelectedShoppingList = shoppingList;

			OnRecordCreated<ShoppingList>(shoppingList.Name);
			OnChangeViewModel(new ChangeViewModelEventArgs { ViewModel = _shoppingListViewModel });
		}

		private void AddToShoppingList(object obj)
		{
			var shoppingList = obj as ShoppingList;
			if (shoppingList == null)
				return;

			shoppingList = _shoppingListRepository.Get(x => x.Id == shoppingList.Id, x => x.ShoppingListRecipes, x => x.ShoppingListIngredients);

			foreach (var recipe in SelectedRecipes)
			{
				_recipeRepository.Get(r => r.Id == recipe.Id, r => r.RecipeIngredients);
				var shoppingListRecipe = new ShoppingListRecipe
				{
					Recipe = recipe,
					ShoppingList = shoppingList
				};
				shoppingList.ShoppingListRecipes.Add(shoppingListRecipe);
			}

			List<RecipeIngredient> allRecipeIngredients = SelectedRecipes.SelectMany(r => r.RecipeIngredients).ToList();
			foreach (var recipeIngredient in allRecipeIngredients)
			{
				var shoppingListIngredient = shoppingList.ShoppingListIngredients.FirstOrDefault(s => s.IngredientId == recipeIngredient.IngredientId);
				if (shoppingListIngredient == null)
				{
					shoppingListIngredient = new ShoppingListIngredient();
					_mapper.Map(recipeIngredient, shoppingListIngredient, nameof(recipeIngredient.Id));
					shoppingList.ShoppingListIngredients.Add(shoppingListIngredient);
					continue;
				}

				shoppingListIngredient.Quantity += recipeIngredient.Quantity;
				shoppingListIngredient.Weight += recipeIngredient.Weight;
			}

			_shoppingListRepository.Update(shoppingList);
			_shoppingListRepository.SaveChanges();

			_shoppingListViewModel.SelectedShoppingList = shoppingList;

			OnRecordUpdated<ShoppingList>(shoppingList.Name);
			OnChangeViewModel(new ChangeViewModelEventArgs { ViewModel = _shoppingListViewModel });
		}

		private bool Filter(object obj)
		{
			if (string.IsNullOrEmpty(SearchText) && !IsFilterEnabled)
			{
				return true;
			}

			var recipeObj = (Recipe)obj;
			Recipe recipe = _recipeRepository.Get(r => r.Id == recipeObj.Id, r => r.Category, r => r.RecipeIngredients);

			bool hasName = true;
			if (!string.IsNullOrEmpty(SearchText))
			{
				hasName = recipe.Name.ToLower().Contains(SearchText.ToLower());
			}

			if (IsFilterEnabled)
			{
				bool hasCategory = true;
				bool hasIngredient = true;

				if (!string.IsNullOrEmpty(SelectedCategory))
				{
					hasCategory = recipe.Category.Name == SelectedCategory;
				}

				if (!string.IsNullOrEmpty(SelectedIngredient))
				{
					hasIngredient = recipe.RecipeIngredients.Select(i => i.Ingredient.Name).Contains(SelectedIngredient);
				}

				if (hasName && hasCategory && hasIngredient)
				{
					return true;
				}

				return false;
			}

			return hasName;
		}


		private void DoubleClick(object obj)
		{
			Edit(obj);
		}

		public override void Refresh()
		{
			var recipes = _recipeRepository.GetAll();
			Recipes.Clear();

			foreach (Recipe recipe in recipes)
			{
				Recipes.Add(recipe);
			}

			SelectedRecipes.Clear();

			Categories = _categoryRepository.GetAll().Select(c => c.Name).ToList();
			Ingredients = _ingredientRepository.GetAll().Select(i => i.Name).ToList();

			var shoppingLists = _shoppingListRepository.GetAll();
			ShoppingLists.Clear();

			foreach (ShoppingList list in shoppingLists)
			{
				ShoppingLists.Add(list);
			}

			base.Refresh();
		}
	}
}
