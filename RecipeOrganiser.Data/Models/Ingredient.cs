namespace RecipeOrganiser.Data.Models
{
	/// <summary>
	/// An ingredient which can be used when creating recipes
	/// </summary>
	public class Ingredient :BaseModel
	{
		/// <summary>
		/// Name of the ingredient
		/// </summary>
		public string Name { get; set; }
	}
}
