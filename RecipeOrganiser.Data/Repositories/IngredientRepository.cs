using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;

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
