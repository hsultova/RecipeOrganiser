using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;

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
