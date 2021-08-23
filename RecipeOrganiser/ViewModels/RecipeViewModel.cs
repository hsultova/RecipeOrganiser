using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Services.Abstract;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class RecipeViewModel : BaseViewModel
	{
		private const string PlaceholderImagePath = "../../../Images/image-placeholder.png";
		private readonly byte[] PlaceholderImageData;

		private readonly IRecipeService _recipeService;
		private readonly ICategoryService _categoryService;
		private readonly IngredientDTO _ingredientDTO;

		public RecipeViewModel(IRecipeService recipeService, ICategoryService categoryService)
		{
			_recipeService = recipeService;
			_categoryService = categoryService;

			_ingredientDTO = new IngredientDTO
			{
				Ingredients = _recipeService.GetIngredients(),
				UnitsOfMeasurement = recipeService.GetUnits()
			};

			PlaceholderImageData = File.ReadAllBytes(PlaceholderImagePath);
			Image = PlaceholderImageData;
		}

		#region Properties

		public int Id { get; set; }

		private string _title;
		public string Title
		{
			get => _title;
			set => SetBackingFieldProperty<string>(ref _title, value, nameof(Title));
		}

		private string _name;
		[Required]
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
					CanExit = false;
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
					CanExit = false;
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
					CanExit = false;
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
					value = PlaceholderImageData;

				if (SetBackingFieldProperty<byte[]>(ref _image, value, nameof(Image)))
				{
					CanExit = false;
				}
			}
		}

		private string _categoryName;
		[Required]
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
					CanExit = false;
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
					CanExit = false;
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
					_categories = new ObservableCollection<Category>(_categoryService.GetAll());
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
					CanExit = false;
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
		public ICommand DeleteCommand => new RelayCommand(Delete);

		#endregion

		private void AddIngredient(object obj)
		{
			AddIngredientControls.Add(new AddIngredientViewModel(_ingredientDTO));
			CanExit = false;
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

			bool shouldClear = false;
			var recipeId = Id;

			if (recipeId == 0)
			{
				recipeId = _recipeService.Create(Name, Description, Note, Image, Category);
				OnRecordCreated<Recipe>(recipeViewModel.Name);
				shouldClear = true;
			}
			else
			{
				_recipeService.Update(Id, Name, Description, Note, Image, Category);
				OnRecordUpdated<Recipe>(recipeViewModel.Name);
			}

			var recipe = _recipeService.GetWithIngredients(recipeId);
			recipe.RecipeIngredients.Clear();

			foreach (var addIngredientViewModel in AddIngredientControls)
			{
				addIngredientViewModel.SetIngredientIfNew();

				_recipeService.CreateRecipeIngredient(
					recipe,
					addIngredientViewModel.Quantity,
					addIngredientViewModel.Weight,
					addIngredientViewModel.Ingredient,
					addIngredientViewModel.UnitOfMeasurement);
			}

			if (shouldClear)
				Clear();

			Refresh();
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

		private void Delete(object obj)
		{
			var addIngredietnViewModel = obj as AddIngredientViewModel;
			if (addIngredietnViewModel == null)
				return;

			AddIngredientControls.Remove(addIngredietnViewModel);
		}


		public override void Refresh()
		{
			var categories = _categoryService.GetAll();

			Categories.Clear();
			foreach (Category category in categories)
			{
				Categories.Add(category);
			}

			_ingredientDTO.Ingredients = _recipeService.GetIngredients();
			CanExit = true;

			base.Refresh();
		}

		public override void Clear()
		{
			Id = 0;
			Name = string.Empty;
			Description = string.Empty;
			Note = string.Empty;
			CategoryName = string.Empty;
			Category = null;
			Image = PlaceholderImageData;
			AddIngredientControls.Clear();

			CanExit = true;
		}
	}
}
