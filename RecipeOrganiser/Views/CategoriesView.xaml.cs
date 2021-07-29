using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.ViewModels;

namespace RecipeOrganiser.Views
{
	/// <summary>
	/// Interaction logic for CategoriesView.xaml
	/// </summary>
	public partial class CategoriesView : UserControl
	{
		public CategoriesView()
		{
			InitializeComponent();
		}

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var vm = DataContext as CategoriesViewModel;
			if (vm == null || vm.SelectedCategories == null)
				return;

			foreach (var item in e.AddedItems)
			{
				var category = item as Category;
				if (category == null)
					continue;
				vm.SelectedCategories.Add(category);
			}

			foreach (var item in e.RemovedItems)
			{
				var category = item as Category;
				if (category == null)
					continue;
				vm.SelectedCategories.Remove(category);
			}

			if (vm.SelectedCategories.Count != 0)
			{
				vm.IsDeleteEnabled = true;
			}
			else
			{
				vm.IsDeleteEnabled = false;
			}
		}
	}
}
