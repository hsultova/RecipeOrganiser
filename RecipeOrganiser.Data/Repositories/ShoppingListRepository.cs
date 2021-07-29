using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;

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
