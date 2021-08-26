using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Domain.Services.Abstract;
using RecipeOrganiser.Utils;
using RecipeOrganiser.Utils.Events;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class HomeViewModel : BaseViewModel
	{
		private readonly RecipeViewModel _recipeViewModel;
		private readonly ShoppingListViewModel _shoppingListViewModel;

		private readonly IRecipeService _recipeService;
		private readonly ICategoryService _categoryService;
		private readonly IShoppingListService _shoppingListService;

		public HomeViewModel(
			IRecipeService recipeService,
			ICategoryService categoryService,
			IShoppingListService shoppingListService,
			RecipeViewModel recipeViewModel,
			ShoppingListViewModel shoppingListViewModel)
		{
			_recipeService = recipeService;
			_categoryService = categoryService;
			_shoppingListService = shoppingListService;

			_recipeViewModel = recipeViewModel;
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
					_recipes = new ObservableCollection<Recipe>(_recipeService.GetAll());
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
					_categories = _categoryService.GetAll().Select(c => c.Name).ToList();
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
					_ingredients = _recipeService.GetIngredients().Select(i => i.Name).ToList();
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
					_shoppingLists = new ObservableCollection<ShoppingList>(_shoppingListService.GetAll());
				}
				return _shoppingLists;
			}
		}

		private string _searchText;
		public string SearchText
		{
			get => _searchText;
			set => SetBackingFieldProperty<string>(ref _searchText, value, nameof(SearchText));
		}

		private bool _isAdvancedFilterEnabled = false;
		public bool IsFilterEnabled
		{
			get => _isAdvancedFilterEnabled;
			set => SetBackingFieldProperty<bool>(ref _isAdvancedFilterEnabled, value, nameof(IsFilterEnabled));
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
			var recipe = _recipeService.GetFull(((Recipe)obj).Id);

			_recipeViewModel.Id = recipe.Id;
			_recipeViewModel.Name = recipe.Name;
			_recipeViewModel.Description = recipe.Description;
			_recipeViewModel.Note = recipe.Note;
			_recipeViewModel.Image = recipe.Image;
			_recipeViewModel.Category = recipe.Category;
			_recipeViewModel.CategoryName = recipe.Category.Name;
			_recipeViewModel.RecipeIngredients = recipe.RecipeIngredients;
			_recipeViewModel.Title = "Edit Recipe";
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
				_recipeService.Delete(selectedRecipe.Id);
				OnRecordDeleted<Recipe>(selectedRecipe.Name);
			}

			SelectedRecipes.Clear();
			Refresh();
		}

		private void AddNewShoppingList(object obj)
		{
			var shoppingList = _shoppingListService.AddRecipesToShoppingList(SelectedRecipes);

			_shoppingListViewModel.SelectedShoppingList = shoppingList;

			OnRecordCreated<ShoppingList>(shoppingList.Name);
			OnChangeViewModel(new ChangeViewModelEventArgs { ViewModel = _shoppingListViewModel });
		}

		private void AddToShoppingList(object obj)
		{
			var shoppingList = obj as ShoppingList;
			if (shoppingList == null)
				return;

			 _shoppingListService.AddRecipesToShoppingList(shoppingList.Id, SelectedRecipes);

			_shoppingListViewModel.SelectedShoppingList = shoppingList;

			OnRecordUpdated<ShoppingList>(shoppingList.Name);
			OnChangeViewModel(new ChangeViewModelEventArgs { ViewModel = _shoppingListViewModel });
		}

		private bool Filter(object obj)
		{
			var recipeObj = (Recipe)obj;
			return _recipeService.IsVisible(recipeObj.Id, SearchText, SelectedCategory, SelectedIngredient, IsFilterEnabled);
		}

		private void DoubleClick(object obj)
		{
			Edit(obj);
		}

		public override void Refresh()
		{
			var recipes = _recipeService.GetAll();
			Recipes.Clear();

			foreach (Recipe recipe in recipes)
			{
				Recipes.Add(recipe);
			}

			SelectedRecipes.Clear();

			Categories = _categoryService.GetAll().Select(c => c.Name).ToList();
			Ingredients = _recipeService.GetIngredients().Select(i => i.Name).ToList();

			var shoppingLists = _shoppingListService.GetAll();
			ShoppingLists.Clear();

			foreach (ShoppingList list in shoppingLists)
			{
				ShoppingLists.Add(list);
			}

			base.Refresh();
		}
	}
}
