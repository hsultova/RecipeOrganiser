using System.Collections.Generic;
using RecipeOrganiser.Domain.Models;

namespace RecipeOrganiser.Domain.Services.Abstract
{
	public interface IShoppingListService
	{
		IList<ShoppingList> GetAll();
		IList<ShoppingList> GetAllWithIngredients();
		ShoppingList GetWithIngredients(int id);
		int Create(string name, string description);
		void Update(int id, string name, string description);
		void Delete(int id);
		ShoppingList AddRecipeToShoppingList(int recipeId);
		void AddRecipeToShoppingList(int shoppingListId, int recipeId);
		ShoppingList AddRecipesToShoppingList(IEnumerable<Recipe> recipes);
		void AddRecipesToShoppingList(int shoppingListId, IEnumerable<Recipe> recipes);
		void Reload(ShoppingList shoppingList);
	}
}
