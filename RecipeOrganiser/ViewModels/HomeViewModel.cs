using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using RecipeOrganiser.Data.Models;
using RecipeOrganiser.Data.Repositories;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class HomeViewModel : BaseViewModel
	{
		private readonly IRecipeRepository _recipeRepository;

		public HomeViewModel(IRecipeRepository recipeRepository)
		{
			_recipeRepository = recipeRepository;

			Recipes = new ObservableCollection<Recipe>(_recipeRepository.GetAll());
			RecipesView = CollectionViewSource.GetDefaultView(Recipes);
			RecipesView.Filter = Filter;
			SelectedRecipes = new List<Recipe>();
		}

		public ObservableCollection<Recipe> Recipes { get; }

		public List<Recipe> SelectedRecipes { get; set; }

		public ICollectionView RecipesView { get; set; }

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

		private bool _isEditEnabled;
		public bool IsEditEnabled
		{
			get
			{
				return _isEditEnabled;
			}
			set
			{
				SetBackingFieldProperty<bool>(ref _isEditEnabled, value, nameof(IsEditEnabled));
			}
		}

		private bool _isDeleteEnabled;
		public bool IsDeleteEnabled
		{
			get
			{
				return _isDeleteEnabled;
			}
			set
			{
				SetBackingFieldProperty<bool>(ref _isDeleteEnabled, value, nameof(IsDeleteEnabled));
			}
		}

		#region Commands
		public ICommand SearchCommand => new RelayCommand(Search);
		public ICommand DeleteCommand => new RelayCommand(Delete);

		#endregion

		private void Search(object obj)
		{
			RecipesView.Refresh();
		}

		private void Delete(object obj)
		{
			foreach(Recipe selectedRecipe in SelectedRecipes)
			{
				var result = MessageBox.Show($"Are you sure you want to delete '{selectedRecipe.Name}' recipe?", "Confirm", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.No)
				{
					continue;
				}
				_recipeRepository.Delete(selectedRecipe.Id);
			}

			_recipeRepository.SaveChanges();
			OnRecordDeleted<Recipe>();
			SelectedRecipes.Clear();
			Refresh();
		}

		private bool Filter(object obj)
		{
			if (string.IsNullOrEmpty(SearchText))
			{
				return true;
			}

			var recipeObj = (Recipe) obj;
			string searchTextToLower = SearchText.ToLower();

			Recipe recipe = _recipeRepository.Get(r => r.Id == recipeObj.Id, r => r.Category, r => r.RecipeIngredients);
			var ingredients = recipe.RecipeIngredients.Select(i => i.Ingredient.Name.ToLower());

			//Filter by recipe name, category or ingredients
			if (recipe.Name.ToLower().Contains(searchTextToLower) ||
				ingredients.Contains(searchTextToLower) ||
				recipe.Category.Name.ToLower().Contains(searchTextToLower))
			{
				return true;
			}

			return false;
		}

		public override void Refresh()
		{
			var recipes = _recipeRepository.GetAll();
			Recipes.Clear();

			foreach(Recipe recipe in recipes)
			{
				Recipes.Add(recipe);
			}

			base.Refresh();
		}
	}
}
