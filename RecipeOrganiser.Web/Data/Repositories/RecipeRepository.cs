using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Web.Data;

namespace RecipeOrganiser.Data.Repositories
{
	public class RecipeRepository : Repository<Recipe>, IRecipeRepository
	{
		public RecipeRepository(ApplicationDbContext context)
			: base(context)
		{
		}
	}
}
