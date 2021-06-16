using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Data.Repositories
{
	public class ShoppingListRecipeRepository : Repository<ShoppingListRecipe>, IShoppingListRecipeRepository
	{
		public ShoppingListRecipeRepository(RecipeOrganiserDbContext context)
			: base(context)
		{
		}
	}
}
