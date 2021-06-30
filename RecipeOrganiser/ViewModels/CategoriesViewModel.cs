using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using RecipeOrganiser.Data.Models;
using RecipeOrganiser.Data.Repositories;
using RecipeOrganiser.Utils.General;
using RecipeOrganiser.ViewModels.Base;

namespace RecipeOrganiser.ViewModels
{
	public class CategoriesViewModel : BaseViewModel
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoriesViewModel(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;

			Categories = new ObservableCollection<Category>(_categoryRepository.GetAll());
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
					_categoryRepository.Create(category);
					OnRecordCreated<Category>(category.Name);
				}
				else
				{
					_categoryRepository.Update(category);
					OnRecordUpdated<Category>(category.Name);
				}
			}

			_categoryRepository.SaveChanges();
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
				_categoryRepository.Delete(selectedCategory.Id);
				OnRecordDeleted<Category>(selectedCategory.Name);
			}

			_categoryRepository.SaveChanges();
			SelectedCategories.Clear();
			Refresh();
		}

		public override void Refresh()
		{
			base.Refresh();

			var categories = _categoryRepository.GetAll();
			SelectedCategories.Clear();
			Categories.Clear();

			foreach (Category category in categories)
			{
				_categoryRepository.Reload(category);
				Categories.Add(category);
			}
		}
	}
}
