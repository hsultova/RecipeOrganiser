using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Domain.Services.Abstract;
using RecipeOrganiser.Utils.Events;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class ShoppingListViewModel : BaseViewModel
	{
		private readonly IShoppingListService _shoppingListService;

		private readonly EditShoppingListViewModel _editShoppingListViewModel;

		public ShoppingListViewModel(
			IShoppingListService shoppingListService,
			EditShoppingListViewModel editShoppingListViewModel)
		{
			_shoppingListService = shoppingListService;

			_editShoppingListViewModel = editShoppingListViewModel;
		}

		private ObservableCollection<ShoppingList> _shoppingLists;
		public ObservableCollection<ShoppingList> ShoppingLists
		{
			get
			{
				if (_shoppingLists == null)
				{
					_shoppingLists = new ObservableCollection<ShoppingList>(_shoppingListService.GetAllWithIngredients());
				}
				return _shoppingLists;
			}
		}

		private ShoppingList _selectedShoppingList;
		public ShoppingList SelectedShoppingList
		{
			get => _selectedShoppingList;
			set => SetBackingFieldProperty<ShoppingList>(ref _selectedShoppingList, value, nameof(SelectedShoppingList));
		}

		public List<ShoppingList> SelectedShoppingLists { get; internal set; } = new List<ShoppingList>();

		#region Commands
		public ICommand SaveCommand => new RelayCommand(Save);
		public ICommand EditCommand => new RelayCommand(Edit);
		public ICommand DeleteCommand => new RelayCommand(Delete);
		public ICommand CellEditingCommand => new RelayCommand(_ => { CanExit = false; });

		#endregion

		private void Save(object obj)
		{
			foreach (ShoppingList list in ShoppingLists)
			{
				if (list.Id == 0)
				{
					_shoppingListService.Create(list.Name, list.Description);
					OnRecordCreated<ShoppingList>(list.Name);
				}
				else
				{
					_shoppingListService.Update(list.Id, list.Name, list.Description);
					OnRecordUpdated<ShoppingList>(list.Name);
				}
			}
		}

		private void Edit(object obj)
		{
			var shoppingList = _shoppingListService.GetWithIngredients(SelectedShoppingList.Id);

			_editShoppingListViewModel.Id = shoppingList.Id;
			_editShoppingListViewModel.CurrentShoppingList = shoppingList;
			_editShoppingListViewModel.ShoppingListIngredients = shoppingList.ShoppingListIngredients;
			OnChangeViewModel(new ChangeViewModelEventArgs { ViewModel = _editShoppingListViewModel });
		}

		private void Delete(object obj)
		{
			foreach (ShoppingList selectedList in SelectedShoppingLists)
			{
				var result = MessageBox.Show($"Are you sure you want to delete '{selectedList.Name}' shopping list? All items in this list will be deleted.", "Confirm", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.No)
				{
					continue;
				}
				_shoppingListService.Delete(selectedList.Id);
				OnRecordDeleted<ShoppingList>(selectedList.Name);
			}

			SelectedShoppingLists.Clear();
			Refresh();
		}

		public override void Refresh()
		{
			base.Refresh();

			var shoppingLists = _shoppingListService.GetAll();
			ShoppingLists.Clear();
			SelectedShoppingLists.Clear();

			foreach (ShoppingList list in shoppingLists)
			{
				_shoppingListService.Reload(list);
				ShoppingLists.Add(list);
			}
		}
	}
}
