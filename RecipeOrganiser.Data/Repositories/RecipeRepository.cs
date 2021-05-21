using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Models;

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
