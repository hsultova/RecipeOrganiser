using System.Collections.Generic;
using RecipeOrganiser.Domain.Models;

namespace RecipeOrganiser.Domain.Services.Abstract
{
	public interface ICategoryService
	{
		IList<Category> GetAll();
		public int Create(string name, string description);
		public void Update(int id, string name, string description);
		void Delete(int id);
		void Reload(Category category);
	}
}
