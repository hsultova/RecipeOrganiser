using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Web.Data;

namespace RecipeOrganiser.Data.Repositories
{
	public class ShoppingListRepository : Repository<ShoppingList>, IShoppingListRepository
	{
		public ShoppingListRepository(ApplicationDbContext context)
			: base(context)
		{

		}
	}
}
