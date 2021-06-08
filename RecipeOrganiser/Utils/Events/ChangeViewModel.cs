using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.Utils.Events
{
	/// <summary>
	/// Event arguments for  ChangeViewModel event.
	/// </summary>
	public class ChangeViewModelEventArgs
	{
		public BaseViewModel ViewModel { get; set; }
	}
}
