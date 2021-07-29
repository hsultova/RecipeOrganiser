namespace RecipeOrganiser.Domain.Models
{
	public class ShoppingListRecipe : BaseModel
	{
		/// <summary>
		/// Recipe of the shopping list recipe
		/// </summary>
		public int RecipeId { get; set; }
		public virtual Recipe Recipe { get; set; }

		/// <summary>
		/// Shopping list 
		/// </summary>
		public int ShoppingListId { get; set; }
		public virtual ShoppingList ShoppingList { get; set; }
	}
}
