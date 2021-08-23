using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Services.Abstract;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class EditShoppingListViewModel : BaseViewModel
	{
		private readonly IShoppingListService _shoppingListService;
		private readonly IRecipeService _recipeService;

		private readonly IngredientDTO _ingredientDTO;

		public EditShoppingListViewModel(IShoppingListService shoppingListService, IRecipeService recipeService)
		{
			_shoppingListService = shoppingListService;
			_recipeService = recipeService;

			_ingredientDTO = new IngredientDTO
			{
				Ingredients = _recipeService.GetIngredients(),
				UnitsOfMeasurement = _recipeService.GetUnits()
			};
		}

		public int Id { get; set; }

		/// <summary>
		/// Used when loading ShoppingListIngredients from the DB
		/// </summary>
		private ICollection<ShoppingListIngredient> _shoppingListIngredients = new List<ShoppingListIngredient>();
		public ICollection<ShoppingListIngredient> ShoppingListIngredients
		{
			get
			{
				return _shoppingListIngredients;
			}
			internal set
			{
				_shoppingListIngredients = value;
				AddIngredientControls.Clear();
				foreach (var ingredient in ShoppingListIngredients)
				{
					var addIngredientViewModel = new AddIngredientViewModel(_ingredientDTO);
					addIngredientViewModel.IngredientName = ingredient.Ingredient.Name;
					addIngredientViewModel.Quantity = ingredient.Quantity;
					addIngredientViewModel.Weight = ingredient.Weight;
					addIngredientViewModel.UnitOfMeasurementName = ingredient.UnitOfMeasurement.Name;

					AddIngredientControls.Add(addIngredientViewModel);
				}
			}
		}

		private ObservableCollection<AddIngredientViewModel> _addIngredientControls = new ObservableCollection<AddIngredientViewModel>();
		public ObservableCollection<AddIngredientViewModel> AddIngredientControls
		{
			get
			{
				return _addIngredientControls;
			}
			set
			{
				if (SetBackingFieldProperty<ObservableCollection<AddIngredientViewModel>>(ref _addIngredientControls, value, nameof(AddIngredientControls)))
				{
					CanExit = false;
				}
			}
		}

		public ShoppingList CurrentShoppingList { get; set; }


		#region Commands
		public ICommand AddIngredientCommand => new RelayCommand(AddIngredient);

		public ICommand SaveCommand => new RelayCommand(Save);
		public ICommand DeleteCommand => new RelayCommand(Delete);

		#endregion

		private void AddIngredient(object obj)
		{
			AddIngredientControls.Add(new AddIngredientViewModel(_ingredientDTO));
			CanExit = false;
		}

		private void Save(object obj)
		{
			if (CurrentShoppingList.ShoppingListIngredients == null)
			{
				CurrentShoppingList.ShoppingListIngredients = new List<ShoppingListIngredient>();
			}

			CurrentShoppingList.ShoppingListIngredients.Clear();
			foreach (var addIngredientViewModel in AddIngredientControls)
			{
				addIngredientViewModel.SetIngredientIfNew();

				var shopppingListIngredient = new ShoppingListIngredient
				{
					Ingredient = addIngredientViewModel.Ingredient,
					Quantity = addIngredientViewModel.Quantity,
					UnitOfMeasurement = addIngredientViewModel.UnitOfMeasurement,
					Weight = addIngredientViewModel.Weight
				};
				CurrentShoppingList.ShoppingListIngredients.Add(shopppingListIngredient);
			}

			OnRecordUpdated<ShoppingList>(CurrentShoppingList.Name);
			Refresh();

			_shoppingListService.Update(CurrentShoppingList.Id, CurrentShoppingList.Name, CurrentShoppingList.Description);
		}

		private void Delete(object obj)
		{
			var addIngredietnViewModel = obj as AddIngredientViewModel;
			if (addIngredietnViewModel == null)
				return;

			AddIngredientControls.Remove(addIngredietnViewModel);
		}

		public override void Refresh()
		{
			_ingredientDTO.Ingredients = _recipeService.GetIngredients();
			CanExit = true;

			base.Refresh();
		}
	}
}
