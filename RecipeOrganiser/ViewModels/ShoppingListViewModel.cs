using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RecipeOrganiser.Data.Models;
using RecipeOrganiser.Data.Repositories;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class ShoppingListViewModel : BaseViewModel
	{
		private readonly IShoppingListRepository _shoppingListRepository;

		private readonly IRecipeRepository _recipeRepository;

		public ShoppingListViewModel(IShoppingListRepository shoppingListRepository,
			IRecipeRepository recipeRepository)
		{
			_shoppingListRepository = shoppingListRepository;
			_recipeRepository = recipeRepository;
			_recipeRepository.GetAll(null, x => x.RecipeIngredients);
		}

		private ObservableCollection<ShoppingList> _shoppingLists;
		public ObservableCollection<ShoppingList> ShoppingLists
		{
			get
			{
				if (_shoppingLists == null)
				{
					_shoppingLists = new ObservableCollection<ShoppingList>(_shoppingListRepository.GetAll(null, x => x.ShoppingListRecipes));
				}
				return _shoppingLists;
			}
		}


		//public List<ShoppingListRecipe> ShoppingListRecipes { get; set; }

		private ShoppingList _selectedShoppingList;
		public ShoppingList SelectedShoppingList
		{
			get
			{
				return _selectedShoppingList;
			}
			set
			{
				SetBackingFieldProperty<ShoppingList>(ref _selectedShoppingList, value, nameof(SelectedShoppingList));
				//if (ShoppingListRecipes == null)
				//{
				//	_shoppingListRepository.Get(x => x.Id == SelectedShoppingList.Id, x => x.ShoppingListRecipes);
				//	ShoppingListRecipes = SelectedShoppingList.ShoppingListRecipes.ToList();
				//	foreach(var shoppingListRecipe in ShoppingListRecipes)
				//	{
				//		_recipeRepository.Get(x => x.Id == shoppingListRecipe.RecipeId, x=>x.RecipeIngredients);
				//		RecipeIngredients.AddRange(shoppingListRecipe.Recipe.RecipeIngredients);
				//	}
				//}
			}
		}

		public List<ShoppingList> SelectedShoppingLists { get; internal set; }

		//public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

		#region Commands
		public ICommand SaveCommand => new RelayCommand(Save);

		public ICommand DeleteCommand => new RelayCommand(Delete);

		#endregion

		private void Save(object obj)
		{

		}

		private void Delete(object obj)
		{

		}

	}
}
