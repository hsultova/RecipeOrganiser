using System.Windows.Input;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class ApplicationViewModel : BaseViewModel
	{
		public ApplicationViewModel()
		{
			CurrentViewModel = new AddIngredientViewModel();
		}

		private BaseViewModel _currentViewModel;
		public BaseViewModel CurrentViewModel
		{
			get
			{
				return _currentViewModel;
			}
			set
			{
				_currentViewModel = value;
				OnPropertyChanged(nameof(CurrentViewModel));
			}
		}

		#region Commands
		public ICommand HomeCommand => new RelayCommand(_ => { CurrentViewModel = new HomeViewModel(); });
		public ICommand NewRecipeCommand => new RelayCommand(_ => { CurrentViewModel = new NewRecipeViewModel(); });
		#endregion
	}
}
