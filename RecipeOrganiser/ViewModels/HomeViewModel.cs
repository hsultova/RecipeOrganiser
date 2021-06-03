﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
		}

		public ObservableCollection<Recipe> Recipes { get; }

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

		#region Commands
		public ICommand SearchCommand => new RelayCommand(Search);

		#endregion

		private void Search(object obj)
		{
			RecipesView.Refresh();
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
