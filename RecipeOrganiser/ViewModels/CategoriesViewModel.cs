﻿using System.Collections.Generic;
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

		#endregion

		private void Save(object obj)
		{
			foreach (Category category in Categories)
			{
				if (category.Id == 0)
				{
					_categoryRepository.Create(category);
				}
				else
				{
					_categoryRepository.Update(category);
				}
			}

			_categoryRepository.SaveChanges();
		}

		private void Delete(object obj)
		{
			foreach (Category selectedCategory in SelectedCategories)
			{
				var result = MessageBox.Show($"Are you sure you want to delete '{selectedCategory.Name}' recipe?", "Confirm", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.No)
				{
					continue;
				}
				_categoryRepository.Delete(selectedCategory.Id);
			}

			_categoryRepository.SaveChanges();
			OnRecordDeleted<Category>();
			SelectedCategories.Clear();
			Refresh();
		}

		public override void Refresh()
		{
			base.Refresh();

			var categories = _categoryRepository.GetAll();
			Categories.Clear();

			foreach (Category category in categories)
			{
				Categories.Add(category);
			}
		}
	}
}