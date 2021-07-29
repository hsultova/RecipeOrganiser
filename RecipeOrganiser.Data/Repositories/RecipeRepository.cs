using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;

namespace RecipeOrganiser.Data.Repositories
{
	public class RecipeRepository : Repository<Recipe>, IRecipeRepository
	{
		public RecipeRepository(RecipeOrganiserDbContext context)
			: base(context)
		{
		}
	}
}
