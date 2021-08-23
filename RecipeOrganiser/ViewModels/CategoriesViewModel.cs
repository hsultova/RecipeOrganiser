using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Services.Abstract;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class CategoriesViewModel : BaseViewModel
	{
		private readonly ICategoryService _categoryService;

		public CategoriesViewModel(ICategoryService categoryService)
		{
			_categoryService = categoryService;

			Categories = new ObservableCollection<Category>(_categoryService.GetAll());
			SelectedCategories = new List<Category>();
		}

		public List<Category> SelectedCategories { get; set; }

		public ObservableCollection<Category> Categories { get; }

		public bool IsDeleteEnabled { get; internal set; }

		#region Commands
		public ICommand SaveCommand => new RelayCommand(Save);

		public ICommand DeleteCommand => new RelayCommand(Delete);

		public ICommand CellEditingCommand => new RelayCommand(_ => { CanExit = false; });

		#endregion

		private void Save(object obj)
		{
			foreach (Category category in Categories)
			{
				if (category.Id == 0)
				{
					_categoryService.Create(category.Name, category.Description);
					OnRecordCreated<Category>(category.Name);
				}
				else
				{
					_categoryService.Update(category.Id, category.Name, category.Description);
					OnRecordUpdated<Category>(category.Name);
				}
			}

			CanExit = true;
		}

		private void Delete(object obj)
		{
			foreach (Category selectedCategory in SelectedCategories)
			{
				var result = MessageBox.Show($"Are you sure you want to delete '{selectedCategory.Name}' category? All items in this category will be deleted.", "Confirm", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.No)
				{
					continue;
				}
				_categoryService.Delete(selectedCategory.Id);
				OnRecordDeleted<Category>(selectedCategory.Name);
			}

			SelectedCategories.Clear();
			Refresh();
		}

		public override void Refresh()
		{
			base.Refresh();

			var categories = _categoryService.GetAll();
			SelectedCategories.Clear();
			Categories.Clear();

			foreach (Category category in categories)
			{
				_categoryService.Reload(category);
				Categories.Add(category);
			}
		}
	}
}
