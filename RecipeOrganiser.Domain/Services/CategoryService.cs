using System.Collections.Generic;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Domain.Services.Abstract;

namespace RecipeOrganiser.Domain.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryService(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public IList<Category> GetAll()
		{
			return _categoryRepository.GetAll();
		}

		public int Create(
			string name,
			string description)
		{
			var category = new Category
			{
				Name = name,
				Description = description
			};

			_categoryRepository.Create(category);
			_categoryRepository.SaveChanges();

			return category.Id;
		}

		public void Update(
			int id,
			string name,
			string description)
		{
			var category = _categoryRepository.Get(r => r.Id == id);

			category.Name = name;
			category.Description = description;

			_categoryRepository.Update(category);
			_categoryRepository.SaveChanges();
		}

		public void Delete(int id)
		{
			_categoryRepository.Delete(id);
			_categoryRepository.SaveChanges();
		}

		public void Reload(Category category)
		{
			_categoryRepository.Reload(category);
		}
	}
}
