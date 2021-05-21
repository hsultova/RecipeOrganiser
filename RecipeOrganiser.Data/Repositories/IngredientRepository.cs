using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Data.Repositories
{
	public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
	{
		public IngredientRepository(RecipeOrganiserDbContext context) 
			: base(context)
		{
		}
	}
}
