namespace RecipeOrganiser.Data.Models
{
	/// <summary>
	/// Shopping list ingredient is used in shopping lists and has quantity or weight.
	/// </summary>
	public class ShoppingListIngredient : BaseModel
	{
		/// <summary>
		/// Quantity of the ingredient. For example 2 carrots, 1 onion...
		/// </summary>
		public int Quantity { get; set; }
		/// <summary>
		/// Weight of the ingredient. For example 2 kg potatoes.
		/// </summary>
		public double Weight { get; set; }

		/// <summary>
		/// Indicates if the ingredient is bought.
		/// </summary>
		public bool IsBought { get; set; } = false;

		/// <summary>
		/// Shopping list in which is the ingredient.
		/// </summary>
		public int ShoppingListId { get; set; }
		public virtual ShoppingList ShoppingList { get; set; }

		/// <summary>
		/// Ingredient of the shopping list ingredient. For example, carrot, onion, photato.
		/// </summary>
		public int IngredientId { get; set; }
		public virtual Ingredient Ingredient { get; set; }

		/// <summary>
		/// Unit of measurement for the ingredient. For example kg.
		/// </summary>
		public int UnitOfMeasurementId { get; set; }
		public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
	}
}
