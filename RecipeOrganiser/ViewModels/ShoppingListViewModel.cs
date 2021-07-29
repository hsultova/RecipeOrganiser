﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Utils.Events;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class ShoppingListViewModel : BaseViewModel
	{
		private readonly IShoppingListRepository _shoppingListRepository;

		private readonly EditShoppingListViewModel _editShoppingListViewModel;

		public ShoppingListViewModel(
			IShoppingListRepository shoppingListRepository,
			EditShoppingListViewModel editShoppingListViewModel)
		{
			_shoppingListRepository = shoppingListRepository;

			_editShoppingListViewModel = editShoppingListViewModel;
		}

		private ObservableCollection<ShoppingList> _shoppingLists;
		public ObservableCollection<ShoppingList> ShoppingLists
		{
			get
			{
				if (_shoppingLists == null)
				{
					_shoppingLists = new ObservableCollection<ShoppingList>(_shoppingListRepository.GetAll(null, x => x.ShoppingListIngredients));
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
					_shoppingListRepository.Create(list);
					OnRecordCreated<ShoppingList>(list.Name);
				}
				else
				{
					_shoppingListRepository.Update(list);
					OnRecordUpdated<ShoppingList>(list.Name);
				}
			}

			_shoppingListRepository.SaveChanges();
		}

		private void Edit(object obj)
		{
			var shoppingList = _shoppingListRepository.Get(r => r.Id == SelectedShoppingList.Id, r => r.ShoppingListIngredients);

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
				_shoppingListRepository.Delete(selectedList.Id);
				OnRecordDeleted<ShoppingList>(selectedList.Name);
			}

			_shoppingListRepository.SaveChanges();
			SelectedShoppingLists.Clear();
			Refresh();
		}

		public override void Refresh()
		{
			base.Refresh();

			var shoppingLists = _shoppingListRepository.GetAll();
			ShoppingLists.Clear();
			SelectedShoppingLists.Clear();

			foreach (ShoppingList list in shoppingLists)
			{
				_shoppingListRepository.Reload(list);
				ShoppingLists.Add(list);
			}
		}
	}
}
