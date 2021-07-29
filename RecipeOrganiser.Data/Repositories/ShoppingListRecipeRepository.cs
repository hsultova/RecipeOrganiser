using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;

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
