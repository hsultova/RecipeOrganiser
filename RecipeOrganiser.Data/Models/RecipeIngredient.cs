namespace RecipeOrganiser.Data.Models
{
	/// <summary>
	/// A recipe ingedient is used in recipes and can has quantity and weight.
	/// </summary>
	public class RecipeIngredient : BaseModel
	{
		/// <summary>
		/// Quantity of the recipe ingredient. For example 2 carrots, 1 onion...
		/// </summary>
		public int Quantity { get; set; }
		/// <summary>
		/// Weight of the recipe ingredient. For example 2 kg potatoes.
		/// </summary>
		public double Weight { get; set; }

		/// <summary>
		/// Recipe in which is the recipe ingredient
		/// </summary>
		public int RecipeId { get; set; }
		public virtual Recipe Recipe { get; set; }

		/// <summary>
		/// Ingredient of the recipe ingredient. For example, carrot, onion, photato
		/// </summary>
		public int IngredientId { get; set; }
		public virtual Ingredient Ingredient { get; set; }

		/// <summary>
		/// Unit of measurement for the recipe ingredient. For example kg
		/// </summary>
		public int UnitOfMeasurementId { get; set; }
		public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
	}
}
