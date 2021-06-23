using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RecipeOrganiser.Data.Models;
using RecipeOrganiser.Data.Repositories;
using RecipeOrganiser.Utils;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class EditShoppingListViewModel : BaseViewModel
	{
		private readonly IMapper _mapper;

		private readonly IShoppingListRepository _shoppingListRepository;
		private readonly IIngredientRepository _ingredientRepository;
		private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

		private readonly IngredientDTO _ingredientDTO;

		public EditShoppingListViewModel(
			IMapper mapper,
			IShoppingListRepository shoppingListRepository,
			IIngredientRepository ingredientRepository,
			IUnitOfMeasurementRepository unitOfMeasurementRepository)
		{
			_mapper = mapper;

			_shoppingListRepository = shoppingListRepository;
			_ingredientRepository = ingredientRepository;
			_unitOfMeasurementRepository = unitOfMeasurementRepository;

			_ingredientDTO = new IngredientDTO
			{
				Ingredients = _ingredientRepository.GetAll(),
				UnitsOfMeasurement = _unitOfMeasurementRepository.GetAll()
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
				SetBackingFieldProperty<ObservableCollection<AddIngredientViewModel>>(ref _addIngredientControls, value, nameof(AddIngredientControls));
			}
		}

		#region Commands
		public ICommand AddIngredientCommand => new RelayCommand(AddIngredient);

		public ICommand SaveCommand => new RelayCommand(Save);

		#endregion

		private void AddIngredient(object obj)
		{
			AddIngredientControls.Add(new AddIngredientViewModel(_ingredientDTO));
		}

		private void Save(object obj)
		{
			var shoppingList = _shoppingListRepository.Get(x => x.Id == Id, x => x.ShoppingListIngredients);

			if (shoppingList.ShoppingListIngredients == null)
			{
				shoppingList.ShoppingListIngredients = new List<ShoppingListIngredient>();
			}

			foreach (var addIngredientViewModel in AddIngredientControls)
			{
				addIngredientViewModel.SetIngredientIfNew();

				var shopppingListIngredient = shoppingList.ShoppingListIngredients.FirstOrDefault(i => i.IngredientId == addIngredientViewModel.Ingredient.Id);
				if (shopppingListIngredient == null)
				{
					shopppingListIngredient = new ShoppingListIngredient();
				}

				_mapper.Map(addIngredientViewModel, shopppingListIngredient);
				shoppingList.ShoppingListIngredients.Add(shopppingListIngredient);
			}

			OnRecordUpdated<ShoppingList>();

			_shoppingListRepository.Update(shoppingList);
			_shoppingListRepository.SaveChanges();
		}
	}
}
