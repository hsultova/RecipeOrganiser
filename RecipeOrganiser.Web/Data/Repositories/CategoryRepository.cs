using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Web.Data;

namespace RecipeOrganiser.Data.Repositories
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(ApplicationDbContext context) 
			: base(context)
		{
		}
	}
}
