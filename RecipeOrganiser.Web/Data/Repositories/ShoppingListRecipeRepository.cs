using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Web.Data;

namespace RecipeOrganiser.Data.Repositories
{
	public class ShoppingListRecipeRepository : Repository<ShoppingListRecipe>, IShoppingListRecipeRepository
	{
		public ShoppingListRecipeRepository(ApplicationDbContext context)
			: base(context)
		{
		}
	}
}
