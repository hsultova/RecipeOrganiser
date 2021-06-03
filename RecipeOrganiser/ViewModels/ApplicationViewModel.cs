using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RecipeOrganiser.Controls;
using RecipeOrganiser.Utils.Events;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class ApplicationViewModel : BaseViewModel
	{
		private readonly Dictionary<BaseViewModel, NavigationMenuItem> _navigationMappings;

		private readonly NewRecipeViewModel _newRecipeViewModel;
		private readonly HomeViewModel _homeViewModel;

		public ApplicationViewModel(
			NewRecipeViewModel newRecipeViewModel,
			HomeViewModel homeViewModel)
		{
			_newRecipeViewModel = newRecipeViewModel;
			_homeViewModel = homeViewModel;
			CurrentViewModel = _homeViewModel;

			CreateNavigationMenu(out List<NavigationMenuItem> items, out Dictionary<BaseViewModel, NavigationMenuItem> navigationMappings);
			Items = items;
			_navigationMappings = navigationMappings;

			CurrentViewModel.DisplayMessageHandler += DisplayMessage;
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
				if (_currentViewModel != null && !_currentViewModel.CanExit())
				{
					var result = MessageBox.Show("Are you sure you want to exit? Any unsaved changes will be lost.", "Confirm", MessageBoxButton.YesNo);
					if (result == MessageBoxResult.No)
					{
						SelectedMenuItem = _navigationMappings[CurrentViewModel];
						return;
					}
					_currentViewModel.Clear();
				}

				if(SetBackingFieldProperty<BaseViewModel>(ref _currentViewModel, value, nameof(CurrentViewModel)))
				{
					CurrentViewModel.Refresh();
				}
			}
		}

		private void DisplayMessage(object sender, DisplayMessageEventArgs e)
		{
			//Todo: Depending on the type, decorate different visual
			//Handle many messages, not only the last
			//Implement closing the message
			StatusMessage = e.Message;
		}

		public IList<NavigationMenuItem> Items { get; }

		private NavigationMenuItem _selectedMenuItem;
		public NavigationMenuItem SelectedMenuItem
		{
			get
			{
				return _selectedMenuItem;
			}
			set
			{
				SetBackingFieldProperty<NavigationMenuItem>(ref _selectedMenuItem, value, nameof(SelectedMenuItem));
			}
		}

		private string _statusMessage;
		public string StatusMessage
		{
			get
			{
				return _statusMessage;
			}
			set
			{
				SetBackingFieldProperty<string>(ref _statusMessage, value, nameof(StatusMessage));
			}
		}

		#region Commands
		public ICommand HomeCommand => new RelayCommand(_ => { CurrentViewModel = _homeViewModel; });
		public ICommand NewRecipeCommand => new RelayCommand(_ => { CurrentViewModel = _newRecipeViewModel; });
		#endregion

		private void CreateNavigationMenu(out List<NavigationMenuItem> items, out Dictionary<BaseViewModel, NavigationMenuItem> navigationMappings)
		{
			var homeMenuItem = new NavigationMenuItem
			{
				Icon = new BitmapImage(new Uri(@"/RecipeOrganiser;component/Images/home.png", UriKind.Relative)),
				Text = "Home",
				SelectionCommand = HomeCommand,
				ToolTip = "Home"
			};
			var newRecipeMenuItem = new NavigationMenuItem
			{
				Icon = new BitmapImage(new Uri(@"/RecipeOrganiser;component/Images/plus.png", UriKind.Relative)),
				Text = "New Recipe",
				SelectionCommand = NewRecipeCommand,
				ToolTip = "New Recipe"
			};

			items = new List<NavigationMenuItem>
			{
				homeMenuItem,
				newRecipeMenuItem
			};

			navigationMappings = new Dictionary<BaseViewModel, NavigationMenuItem>
			{
				{ _homeViewModel, homeMenuItem },
				{ _newRecipeViewModel, newRecipeMenuItem }
			};
		}
	}
}
