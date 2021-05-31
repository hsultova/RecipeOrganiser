﻿using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RecipeOrganiser.ViewModels;

namespace RecipeOrganiser.Views
{
	/// <summary>
	/// Interaction logic for NewRecipeView.xaml
	/// </summary>
	public partial class NewRecipeView : UserControl
	{
		public NewRecipeView()
		{
			InitializeComponent();
		}

		private void Image_Drop(object sender, DragEventArgs e)
		{
			string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

			//Todo: handle more then one file
			var filePath = filePaths.FirstOrDefault();
			byte[] data = File.ReadAllBytes(filePath);

			var vm = (NewRecipeViewModel)this.DataContext;
			vm.Image = data;
			vm.ImagePath = filePath;
		}
	}
}
