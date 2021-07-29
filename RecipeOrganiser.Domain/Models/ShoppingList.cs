using System.Collections.Generic;

namespace RecipeOrganiser.Domain.Models
{
	public class ShoppingList : BaseModel
	{
		/// <summary>
		/// Name of the shopping list.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Description of the shopping list.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// All recipes included in the shopping list.
		/// </summary>
		public virtual ICollection<ShoppingListRecipe> ShoppingListRecipes { get; set; }

		/// <summary>
		/// All ingredients included in the shopping list.
		/// </summary>
		public virtual ICollection<ShoppingListIngredient> ShoppingListIngredients { get; set; }
	}
}
