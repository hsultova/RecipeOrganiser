using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Web.Data;

namespace RecipeOrganiser.Data.Repositories
{
	public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
	{
		public IngredientRepository(ApplicationDbContext context) 
			: base(context)
		{
		}
	}
}
