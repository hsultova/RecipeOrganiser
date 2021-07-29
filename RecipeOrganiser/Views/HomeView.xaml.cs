﻿using System.Windows;
using System.Windows.Controls;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.ViewModels;

namespace RecipeOrganiser.Views
{
	/// <summary>
	/// Interaction logic for HomeView.xaml
	/// </summary>
	public partial class HomeView : UserControl
	{
		private HomeViewModel _homeViewModel;
		public HomeView()
		{
			InitializeComponent();

			_homeViewModel = (HomeViewModel)DataContext;
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (_homeViewModel == null)
			{
				_homeViewModel = (HomeViewModel)DataContext;
			}

			if (_homeViewModel.SelectedRecipes == null)
				return;

			_homeViewModel.SelectedRecipes.Clear();
			foreach (var item in RecipesListBox.SelectedItems)
			{
				_homeViewModel.SelectedRecipes.Add((Recipe)item);
			}

			if (RecipesListBox.SelectedItems.Count != 0)
			{
				DeleteToolbarMenuItem.IsEnabled = true;
				DeleteMenuItem.IsEnabled = true;

				ShoppingListToolbarMenuItem.IsEnabled = true;
				ShoppingListMenuItem.IsEnabled = true;
			}
			else
			{
				DeleteToolbarMenuItem.IsEnabled = false;
				DeleteMenuItem.IsEnabled = false;

				ShoppingListToolbarMenuItem.IsEnabled = false;
				ShoppingListMenuItem.IsEnabled = false;
			}

			if (RecipesListBox.SelectedItems.Count == 1)
			{
				EditToolbarMenuItem.IsEnabled = true;
				EditMenuItem.IsEnabled = true;
			}
			else
			{
				EditToolbarMenuItem.IsEnabled = false;
				EditMenuItem.IsEnabled = false;
			}
		}

		private void AdvancedFilterButton_Checked(object sender, RoutedEventArgs e)
		{
			AdvancedFilter.Visibility = Visibility.Visible;
		}

		private void AdvancedFilterButton_Unchecked(object sender, RoutedEventArgs e)
		{
			AdvancedFilter.Visibility = Visibility.Collapsed;
			ClearFilter();
		}

		private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ClearFilterButton.Visibility != Visibility.Visible)
			{
				ClearFilterButton.Visibility = Visibility.Visible;
			}
		}

		private void IngredientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ClearFilterButton.Visibility != Visibility.Visible)
			{
				ClearFilterButton.Visibility = Visibility.Visible;
			}
		}

		private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
		{
			ClearFilter();
		}

		private void ClearFilter()
		{
			CategoryComboBox.SelectedIndex = -1;
			IngredientComboBox.SelectedIndex = -1;

			ClearFilterButton.Visibility = Visibility.Collapsed;
		}
	}
}
