using System;
using System.Linq;
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
using RecipeOrganiser.ViewModels;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Views
{
	/// <summary>
	/// Interaction logic for HomeView.xaml
	/// </summary>
	public partial class HomeView : UserControl
	{
		public HomeView()
		{
			InitializeComponent();
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var vm = DataContext as HomeViewModel;
			if (vm == null || vm.SelectedRecipes == null)
				return;

			foreach(var item in e.AddedItems)
			{
				vm.SelectedRecipes.Add((Recipe) item);
			}

			foreach (var item in e.RemovedItems)
			{
				vm.SelectedRecipes.Remove((Recipe)item);
			}

			if(vm.SelectedRecipes.Count != 0)
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
