﻿using System.Windows.Controls;
using RecipeOrganiser.Data.Models;
using RecipeOrganiser.ViewModels;

namespace RecipeOrganiser.Views
{
	/// <summary>
	/// Interaction logic for ShoppingListView.xaml
	/// </summary>
	public partial class ShoppingListView : UserControl
	{
		public ShoppingListView()
		{
			InitializeComponent();
		}

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var vm = DataContext as ShoppingListViewModel;
			if (vm == null || vm.SelectedShoppingLists == null)
				return;

			vm.SelectedShoppingLists.Clear();
			foreach (var item in ShoppingListDataGrid.SelectedItems)
			{
				var list = item as ShoppingList;
				if (list == null)
					continue;
				vm.SelectedShoppingLists.Add(list);
			}

			if (ShoppingListDataGrid.SelectedItems.Count != 0)
			{
				DeleteButton.IsEnabled = true;
			}
			else
			{
				DeleteButton.IsEnabled = false;
			}
		}
	}
}
