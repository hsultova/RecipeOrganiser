using System.Windows.Input;
using RecipeOrganiser.Data.Repositories;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class ApplicationViewModel : BaseViewModel
	{
		private readonly NewRecipeViewModel _newRecipeViewModel;
		public ApplicationViewModel(NewRecipeViewModel newRecipeViewModel)
		{
			CurrentViewModel = new AddIngredientViewModel();
			_newRecipeViewModel = newRecipeViewModel;
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
		public ICommand NewRecipeCommand => new RelayCommand(_ => { CurrentViewModel = _newRecipeViewModel; });
		#endregion
	}
}
