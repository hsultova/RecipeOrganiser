﻿using System;
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

		private readonly IRecipeRepository _recipeRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IIngredientRepository _ingredientRepository;

		public HomeViewModel(
			IMapper mapper,
			IRecipeRepository recipeRepository,
			ICategoryRepository categoryRepository,
			IIngredientRepository ingredientRepository,
			RecipeViewModel recipeViewModel)
		{
			_mapper = mapper;

			_recipeRepository = recipeRepository;
			_categoryRepository = categoryRepository;
			_ingredientRepository = ingredientRepository;

			_recipeViewModel = recipeViewModel;
			_recipeViewModel.Title = "Edit Recipe";

			Recipes = new ObservableCollection<Recipe>(_recipeRepository.GetAll());
			RecipesView = CollectionViewSource.GetDefaultView(Recipes);
			RecipesView.Filter = Filter;
			SelectedRecipes = new List<Recipe>();

			Categories = _categoryRepository.GetAll().Select(c => c.Name).ToList();
			Ingredients = _ingredientRepository.GetAll().Select(i => i.Name).ToList();
		}

		public ObservableCollection<Recipe> Recipes { get; }

		public List<Recipe> SelectedRecipes { get; set; }

		private Recipe _selectedRecipe;
		public Recipe SelectedRecipe
		{
			get
			{
				return _selectedRecipe;
			}
			set
			{
				SetBackingFieldProperty<Recipe>(ref _selectedRecipe, value, nameof(SelectedRecipe));
			}
		}

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
				return _ingredients;
			}
			set
			{
				SetBackingFieldProperty<List<string>>(ref _ingredients, value, nameof(Ingredients));
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


		#endregion

		private void Search(object obj)
		{
			RecipesView.Refresh();
		}

		private void Edit(object obj)
		{
			if (SelectedRecipe.RecipeIngredients == null)
			{
				_recipeRepository.Get(r => r.Id == SelectedRecipe.Id, r => r.RecipeIngredients);
			}

			_mapper.Map(SelectedRecipe, _recipeViewModel);
			_recipeViewModel.CurrentRecipe = SelectedRecipe;
			_recipeViewModel.CategoryName = SelectedRecipe.Category.Name;
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
			}

			_recipeRepository.SaveChanges();
			OnRecordDeleted<Recipe>();
			SelectedRecipes.Clear();
			Refresh();
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

			base.Refresh();
		}
	}
}
