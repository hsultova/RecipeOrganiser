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
	public class NewRecipeViewModel : BaseViewModel
	{
		private const string PlaceholderImagePath = "/Images/image-placeholder.png";

		private readonly IMapper _mapper;

		private readonly IRecipeRepository _recipeRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IIngredientRepository _ingredientRepository;
		private readonly IRecipeIngredientRepository _recipeIngredientRepository;
		private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

		private readonly IngredientDTO _ingredientDTO;

		private bool _canExit = true;

		public NewRecipeViewModel(
			IMapper mapper,
			IRecipeRepository recipeRepository,
			ICategoryRepository categoryRepository,
			IIngredientRepository ingredientRepository,
			IRecipeIngredientRepository recipeIngredientRepository,
			IUnitOfMeasurementRepository unitOfMeasurementRepository)
		{
			_mapper = mapper;

			_recipeRepository = recipeRepository;
			_categoryRepository = categoryRepository;
			_ingredientRepository = ingredientRepository;
			_recipeIngredientRepository = recipeIngredientRepository;
			_unitOfMeasurementRepository = unitOfMeasurementRepository;

			_ingredientDTO = new IngredientDTO
			{
				Ingredients = _ingredientRepository.GetAll(),
				UnitsOfMeasurement = _unitOfMeasurementRepository.GetAll()
			};

			Categories = new ObservableCollection<Category>(_categoryRepository.GetAll());
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
				if(SetBackingFieldProperty<string>(ref _description, value, nameof(Description)))
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
				if(SetBackingFieldProperty<string>(ref _note, value, nameof(Note)))
				{
					_canExit = false;
				}
			}
		}

		private string _imagePath = PlaceholderImagePath;
		public string ImagePath
		{
			get
			{
				return _imagePath;
			}
			set
			{
				if(SetBackingFieldProperty<string>(ref _imagePath, value, nameof(ImagePath)))
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
				SetBackingFieldProperty<byte[]>(ref _image, value, nameof(Image));
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
				if(SetBackingFieldProperty<string>(ref _categoryName, value, nameof(CategoryName)))
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
				if(SetBackingFieldProperty<Category>(ref _category, value, nameof(Category)))
				{
					_canExit = false;
				}
			}
		}

		public ObservableCollection<Category> Categories { get; }

		private ObservableCollection<AddIngredientViewModel> _addIngredientControls = new ObservableCollection<AddIngredientViewModel>();
		public ObservableCollection<AddIngredientViewModel> AddIngredientControls
		{
			get
			{
				return _addIngredientControls;
			}
			set
			{
				if(SetBackingFieldProperty<ObservableCollection<AddIngredientViewModel>>(ref _addIngredientControls, value, nameof(AddIngredientControls)))
				{
					_canExit = false;
				}
			}
		}

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
			var recipeViewModel = obj as NewRecipeViewModel;
			if (recipeViewModel == null)
			{
				//The recipe is null, todo log error
				return;
			}

			if (Category == null)
			{
				Category = new Category { Name = CategoryName };
			}

			var recipeIngredients = new List<RecipeIngredient>();
			foreach (AddIngredientViewModel addIngredientViewModel in AddIngredientControls)
			{
				addIngredientViewModel.SetIngredientIfNew();
				addIngredientViewModel.SetUnitOfMeasurementIfNew();

				var recipeIngredient = new RecipeIngredient();
				_mapper.Map(addIngredientViewModel, recipeIngredient);
				recipeIngredients.Add(recipeIngredient);
			}

			var recipe = new Recipe();
			_mapper.Map(recipeViewModel, recipe);
			recipe.RecipeIngredients = recipeIngredients;
			CreateRecipe(recipe);
			Refresh();
			Clear();

			OnRecordCreated<Recipe>();
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
				ImagePath = fileDialog.FileName;
			}
		}

		/// <summary>
		/// Creates a recipe and recipe ingredients. Also, check if navigation properties does not exist, creates them.
		/// </summary>
		/// <param name="recipe">A recipe to create in the DB table</param>
		private void CreateRecipe(Recipe recipe)
		{
			//Check navigation properties
			if (recipe.Category.Id == 0)
			{
				_categoryRepository.Create(Category);
			}

			foreach (RecipeIngredient recipeIngredient in recipe.RecipeIngredients.ToList())
			{
				if (recipeIngredient.Ingredient?.Id == 0)
				{
					_ingredientRepository.Create(recipeIngredient.Ingredient);
				}

				_recipeIngredientRepository.Create(recipeIngredient);
			}

			_recipeRepository.Create(recipe);
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
			ImagePath = PlaceholderImagePath;
			AddIngredientControls.Clear();

			_canExit = true;
		}

		public override bool CanExit()
		{
			return _canExit;
		}
	}
}
