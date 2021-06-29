using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RecipeOrganiser.Controls;
using RecipeOrganiser.Utils.Events;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class ApplicationViewModel : BaseViewModel, IDisposable
	{
		private readonly Dictionary<BaseViewModel, NavigationMenuItem> _navigationMappings;

		private readonly RecipeViewModel _recipeViewModel;
		private readonly HomeViewModel _homeViewModel;
		private readonly CategoriesViewModel _categoriesViewModel;
		private readonly ShoppingListViewModel _shoppingListViewModel;
		private readonly EditShoppingListViewModel _editShoppingListViewModel;

		public ApplicationViewModel(
			RecipeViewModel recipeViewModel,
			HomeViewModel homeViewModel,
			CategoriesViewModel categoriesViewModel,
			ShoppingListViewModel shoppingListViewModel,
			EditShoppingListViewModel editShoppingListViewModel)
		{
			_recipeViewModel = recipeViewModel;
			_recipeViewModel.Title = "New Recipe";

			_homeViewModel = homeViewModel;
			_categoriesViewModel = categoriesViewModel;
			_shoppingListViewModel = shoppingListViewModel;
			_editShoppingListViewModel = editShoppingListViewModel;

			CurrentViewModel = _homeViewModel;

			CreateNavigationMenu(out List<NavigationMenuItem> items, out Dictionary<BaseViewModel, NavigationMenuItem> navigationMappings);
			Items = items;
			_navigationMappings = navigationMappings;

			SubscribeViewModel(_homeViewModel);
			SubscribeViewModel(_recipeViewModel);
			SubscribeViewModel(_categoriesViewModel);
			SubscribeViewModel(_shoppingListViewModel);
			SubscribeViewModel(_editShoppingListViewModel);
		}

		public void SubscribeViewModel(BaseViewModel model)
		{
			model.DisplayMessageHandler += DisplayMessage;
			model.ChangeViewModelHandler += ChangeViewModel;
		}

		public void UnSubscribeViewModel(BaseViewModel model)
		{
			model.ChangeViewModelHandler -= ChangeViewModel;
			model.DisplayMessageHandler -= DisplayMessage;
		}

		public void Dispose()
		{
			UnSubscribeViewModel(_homeViewModel);
			UnSubscribeViewModel(_recipeViewModel);
			UnSubscribeViewModel(_categoriesViewModel);
			UnSubscribeViewModel(_shoppingListViewModel);
			UnSubscribeViewModel(_editShoppingListViewModel);
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
						if(_navigationMappings.ContainsKey(CurrentViewModel))
							SelectedMenuItem = _navigationMappings[CurrentViewModel];
						return;
					}
					_currentViewModel.Clear();
				}

				if (SetBackingFieldProperty<BaseViewModel>(ref _currentViewModel, value, nameof(CurrentViewModel)))
				{
					CurrentViewModel.Refresh();
				}
			}
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

		private ObservableCollection<DisplayMessageEventArgs> _statusMessages = new ObservableCollection<DisplayMessageEventArgs>();
		public ObservableCollection<DisplayMessageEventArgs> StatusMessages
		{
			get
			{
				return _statusMessages;
			}
			set
			{
				SetBackingFieldProperty<ObservableCollection<DisplayMessageEventArgs>>(ref _statusMessages, value, nameof(StatusMessages));
			}
		}

		private void DisplayMessage(object sender, DisplayMessageEventArgs e)
		{
			StatusMessages.Add(e);
		}

		private void ChangeViewModel(object sender, ChangeViewModelEventArgs e)
		{
			CurrentViewModel = e.ViewModel;
		}

		#region Commands
		public ICommand HomeCommand => new RelayCommand(_ => { CurrentViewModel = _homeViewModel; });
		public ICommand RecipeCommand => new RelayCommand(_ => { CurrentViewModel = _recipeViewModel; });
		public ICommand CategoriesCommand => new RelayCommand(_ => { CurrentViewModel = _categoriesViewModel; });
		public ICommand ShoppingListsCommand => new RelayCommand(_ => { CurrentViewModel = _shoppingListViewModel; });
		public ICommand CloseStatusMessageCommand => new RelayCommand(CloseStatusMessage);
		public ICommand ClearMessagesCommand => new RelayCommand(_ => { StatusMessages.Clear(); });

		#endregion

		private void CloseStatusMessage(object obj)
		{
			StatusMessages.Remove((DisplayMessageEventArgs)obj);
		}

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
				SelectionCommand = RecipeCommand,
				ToolTip = "New Recipe"
			};
			var categoriesMenuItem = new NavigationMenuItem
			{
				Icon = new BitmapImage(new Uri(@"/RecipeOrganiser;component/Images/notebook-multiple.png", UriKind.Relative)),
				Text = "Categories",
				SelectionCommand = CategoriesCommand,
				ToolTip = "Categories"
			};
			var shoppingListsMenuItem = new NavigationMenuItem
			{
				Icon = new BitmapImage(new Uri(@"/RecipeOrganiser;component/Images/cart.png", UriKind.Relative)),
				Text = "Shopping Lists",
				SelectionCommand = ShoppingListsCommand,
				ToolTip = "Shopping Lists"
			};

			items = new List<NavigationMenuItem>
			{
				homeMenuItem,
				newRecipeMenuItem,
				categoriesMenuItem,
				shoppingListsMenuItem
			};

			navigationMappings = new Dictionary<BaseViewModel, NavigationMenuItem>
			{
				{ _homeViewModel, homeMenuItem },
				{ _recipeViewModel, newRecipeMenuItem },
				{ _categoriesViewModel, categoriesMenuItem },
				{ _shoppingListViewModel, shoppingListsMenuItem }
			};
		}
	}
}
