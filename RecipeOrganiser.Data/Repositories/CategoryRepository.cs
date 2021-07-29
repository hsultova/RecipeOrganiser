using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;

namespace RecipeOrganiser.Data.Repositories
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(RecipeOrganiserDbContext context) 
			: base(context)
		{
		}
	}
}
