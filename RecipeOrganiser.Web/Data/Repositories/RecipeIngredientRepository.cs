using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Web.Data;

namespace RecipeOrganiser.Data.Repositories
{
	public class RecipeIngredientRepository : Repository<RecipeIngredient>, IRecipeIngredientRepository
	{
		public RecipeIngredientRepository(ApplicationDbContext context)
			: base(context)
		{
		}
	}
}
