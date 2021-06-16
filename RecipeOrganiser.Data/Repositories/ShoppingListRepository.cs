using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Data.Repositories
{
	public class ShoppingListRepository : Repository<ShoppingList>, IShoppingListRepository
	{
		public ShoppingListRepository(RecipeOrganiserDbContext context)
			: base(context)
		{

		}
	}
}
