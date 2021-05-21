using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Data.Repositories
{
	public class RecipeIngredientRepository : Repository<RecipeIngredient>, IRecipeIngredientRepository
	{
		public RecipeIngredientRepository(RecipeOrganiserDbContext context)
			: base(context)
		{
		}
	}
}
