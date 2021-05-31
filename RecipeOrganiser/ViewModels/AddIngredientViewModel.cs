using System.Collections.Generic;
using RecipeOrganiser.Data.Models;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class IngredientDTO
	{
		public IList<Ingredient> Ingredients { get; set; }
		public IList<UnitOfMeasurement> UnitsOfMeasurement { get; set; }
	}

	public class AddIngredientViewModel : BaseViewModel
	{
		private readonly IngredientDTO _ingredientDTO;

		public AddIngredientViewModel(IngredientDTO ingredientDTO)
		{
			_ingredientDTO = ingredientDTO;
		}

		private string _ingredientName;
		public string IngredientName
		{
			get
			{
				return _ingredientName;
			}
			set
			{
				SetBackingFieldProperty<string>(ref _ingredientName, value, nameof(IngredientName));
			}
		}

		private Ingredient _ingredient;
		public Ingredient Ingredient
		{
			get
			{
				return _ingredient;
			}
			set
			{
				SetBackingFieldProperty<Ingredient>(ref _ingredient, value, nameof(Ingredient));
			}
		}

		private string _unitOfMeasurementName;
		public string UnitOfMeasurementName
		{
			get
			{
				return _unitOfMeasurementName;
			}
			set
			{
				SetBackingFieldProperty<string>(ref _unitOfMeasurementName, value, nameof(UnitOfMeasurementName));
			}
		}

		private UnitOfMeasurement _unitOfMeasurement;
		public UnitOfMeasurement UnitOfMeasurement
		{
			get
			{
				return _unitOfMeasurement;
			}
			set
			{
				SetBackingFieldProperty<UnitOfMeasurement>(ref _unitOfMeasurement, value, nameof(UnitOfMeasurement));
			}
		}

		public IList<Ingredient> Ingredients => _ingredientDTO.Ingredients;
		public IList<UnitOfMeasurement> UnitsOfMeasurements => _ingredientDTO.UnitsOfMeasurement;

		private int _quantity;
		public int Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				SetBackingFieldProperty<int>(ref _quantity, value, nameof(Quantity));
			}
		}

		private double _weight;
		public double Weight
		{
			get
			{
				return _weight;
			}
			set
			{
				SetBackingFieldProperty<double>(ref _weight, value, nameof(Weight));
			}
		}

		internal void SetIngredientIfNew()
		{
			if (Ingredient != null || IngredientName == null)
			{
				return;
			}

			Ingredient = new Ingredient
			{
				Name = IngredientName
			};
		}

		internal void SetUnitOfMeasurementIfNew()
		{
			if (UnitOfMeasurement != null || UnitOfMeasurementName == null)
			{
				return;
			}

			UnitOfMeasurement = new UnitOfMeasurement
			{
				Name = UnitOfMeasurementName
			};
		}
	}
}
