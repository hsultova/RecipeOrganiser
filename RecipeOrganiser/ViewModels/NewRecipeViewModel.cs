using System.Collections.ObjectModel;
using System.Windows.Input;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class NewRecipeViewModel : BaseViewModel
	{
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
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
				_description = value;
				OnPropertyChanged(nameof(Description));
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
				_note = value;
				OnPropertyChanged(nameof(Note));
			}
		}

		private string _imagePath = "/Images/image-placeholder.png";
		public string ImagePath
		{
			get
			{
				return _imagePath;
			}
			set
			{
				_imagePath = value;
				OnPropertyChanged(nameof(ImagePath));
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
				_addIngredientControls = value;
				OnPropertyChanged(nameof(AddIngredientControls));
			}
		}


		#region Commands
		public ICommand AddIngredientCommand => new RelayCommand(_ => { AddIngredientControls.Add(new AddIngredientViewModel()); });
		#endregion
	}
}
