using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using RecipeOrganiser.Data.Models;
using RecipeOrganiser.Data.Repositories;
using RecipeOrganiser.Utils;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class RecipeViewModel : BaseViewModel
	{
		private const string PlaceholderImagePath = "../../../Images/image-placeholder.png";
		private readonly byte[] PlaceholderImageData;

		private readonly IMapper _mapper;

		private readonly IRecipeRepository _recipeRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IIngredientRepository _ingredientRepository;
		private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

		private readonly IngredientDTO _ingredientDTO;

		private bool _canExit = true;

		public RecipeViewModel(
			IMapper mapper,
			IRecipeRepository recipeRepository,
			ICategoryRepository categoryRepository,
			IIngredientRepository ingredientRepository,
			IUnitOfMeasurementRepository unitOfMeasurementRepository)
		{
			_mapper = mapper;

			_recipeRepository = recipeRepository;
			_categoryRepository = categoryRepository;
			_ingredientRepository = ingredientRepository;
			_unitOfMeasurementRepository = unitOfMeasurementRepository;

			_ingredientDTO = new IngredientDTO
			{
				Ingredients = _ingredientRepository.GetAll(),
				UnitsOfMeasurement = _unitOfMeasurementRepository.GetAll()
			};

			PlaceholderImageData = File.ReadAllBytes(PlaceholderImagePath);
			Image = PlaceholderImageData;
		}

		#region Properties

		public int Id { get; set; }
		public Recipe CurrentRecipe { get; set; }

		private string _title;
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				SetBackingFieldProperty<string>(ref _title, value, nameof(Title));
			}
		}

		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (SetBackingFieldProperty<string>(ref _name, value, nameof(Name)))
				{
					_canExit = false;
				}
			}
		}

		private string _description;
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (SetBackingFieldProperty<string>(ref _description, value, nameof(Description)))
				{
					_canExit = false;
				}
			}
		}

		private string _note;
		public string Note
		{
			get
			{
				return _note;
			}
			set
			{
				if (SetBackingFieldProperty<string>(ref _note, value, nameof(Note)))
				{
					_canExit = false;
				}
			}
		}

		private byte[] _image;
		public byte[] Image
		{
			get { return _image; }
			set
			{
				if (value == null)
					return;

				if (SetBackingFieldProperty<byte[]>(ref _image, value, nameof(Image)))
				{
					_canExit = false;
				}
			}
		}

		private string _categoryName;
		public string CategoryName
		{
			get
			{
				return _categoryName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
					return;

				if (SetBackingFieldProperty<string>(ref _categoryName, value, nameof(CategoryName)))
				{
					_canExit = false;
				}
			}
		}

		private Category _category;
		public Category Category
		{
			get
			{
				return _category;
			}
			set
			{
				if (SetBackingFieldProperty<Category>(ref _category, value, nameof(Category)))
				{
					_canExit = false;
				}
			}
		}

		private ObservableCollection<Category> _categories;
		public ObservableCollection<Category> Categories
		{
			get
			{
				if (_categories == null)
				{
					_categories = new ObservableCollection<Category>(_categoryRepository.GetAll());
				}
				return _categories;
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
					_canExit = false;
				}
			}
		}

		/// <summary>
		/// Used when loading RecipeIngredients from the DB
		/// </summary>
		private ICollection<RecipeIngredient> _recipeIngredients = new List<RecipeIngredient>();
		public ICollection<RecipeIngredient> RecipeIngredients
		{
			get
			{
				return _recipeIngredients;
			}
			internal set
			{
				_recipeIngredients = value;
				AddIngredientControls.Clear();
				foreach (var ingredient in RecipeIngredients)
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

		#endregion

		#region Commands
		public ICommand AddIngredientCommand => new RelayCommand(AddIngredient);
		public ICommand SaveCommand => new RelayCommand(Save);
		public ICommand ClearCommand => new RelayCommand(Clear);
		public ICommand UploadImageCommand => new RelayCommand(UploadImage);

		#endregion

		private void AddIngredient(object obj)
		{
			AddIngredientControls.Add(new AddIngredientViewModel(_ingredientDTO));
			_canExit = false;
		}

		private void Save(object obj)
		{
			var recipeViewModel = obj as RecipeViewModel;
			if (recipeViewModel == null)
			{
				//The recipe is null, todo log error
				return;
			}

			if (Category == null)
			{
				Category = new Category { Name = CategoryName };
			}

			var recipe = new Recipe();
			bool shouldClear = false;
			if (CurrentRecipe != null)
			{
				recipe = CurrentRecipe;
				_mapper.Map(recipeViewModel, recipe);
				OnRecordUpdated<Recipe>();
			}
			else
			{
				_mapper.Map(recipeViewModel, recipe, nameof(RecipeIngredients));
				shouldClear = true;
				OnRecordCreated<Recipe>();
			}

			if(recipe.RecipeIngredients == null)
			{
				recipe.RecipeIngredients = new List<RecipeIngredient>();
			}

			foreach (var addIngredientViewModel in AddIngredientControls)
			{
				addIngredientViewModel.SetIngredientIfNew();

				var recipeIngredient = recipe.RecipeIngredients.FirstOrDefault(i => i.IngredientId == addIngredientViewModel.Ingredient.Id);
				if(recipeIngredient == null)
				{
					recipeIngredient = new RecipeIngredient();
				}

				_mapper.Map(addIngredientViewModel, recipeIngredient);
				recipe.RecipeIngredients.Add(recipeIngredient);
			}

			CreateOrUpdateRecipe(recipe);

			if (shouldClear)
				Clear();

			Refresh();
			_canExit = true;
		}

		private void Clear(object obj)
		{
			Clear();
		}

		private void UploadImage(object obj)
		{
			var fileDialog = new Microsoft.Win32.OpenFileDialog
			{
				DefaultExt = ".png",
				Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*"
			};

			if (fileDialog.ShowDialog() == true)
			{
				byte[] data = File.ReadAllBytes(fileDialog.FileName);

				Image = data;
			}
		}

		/// <summary>
		/// Creates or updates a recipe.
		/// </summary>
		/// <param name="recipe">A recipe to create or update in the DB table</param>
		private void CreateOrUpdateRecipe(Recipe recipe)
		{
			if (recipe.Id == 0)
			{
				_recipeRepository.Create(recipe);
			}
			else
			{
				_recipeRepository.Update(recipe);
			}

			_recipeRepository.SaveChanges();
		}

		public override void Refresh()
		{
			var categories = _categoryRepository.GetAll();

			Categories.Clear();
			foreach (Category category in categories)
			{
				Categories.Add(category);
			}

			_ingredientDTO.Ingredients = _ingredientRepository.GetAll();

			base.Refresh();
		}

		public override void Clear()
		{
			Name = string.Empty;
			Description = string.Empty;
			Note = string.Empty;
			CategoryName = string.Empty;
			Image = PlaceholderImageData;
			AddIngredientControls.Clear();

			_canExit = true;
		}

		public override bool CanExit()
		{
			return _canExit;
		}
	}
}
