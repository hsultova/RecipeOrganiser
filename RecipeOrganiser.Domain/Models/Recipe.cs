using System.Collections.Generic;

namespace RecipeOrganiser.Domain.Models
{
	/// <summary>
	/// Cooking recipe. Can be added in category. Contains list of ingredients (of type <see cref="RecipeIngredients"/>)
	/// </summary>
	public class Recipe : BaseModel
	{
		/// <summary>
		/// Name of the recipe
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Description of the recipe
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Notes added to the recipe
		/// </summary>
		public string Note { get; set; }
		/// <summary>
		/// Image of the recipe
		/// </summary>
		public byte[] Image { get; set; }

		/// <summary>
		/// Category in which the recipe is added
		/// </summary>
		public int CategoryId { get; set; }
		public virtual Category Category { get; set; }

		/// <summary>
		/// All recipe ingredients
		/// </summary>
		public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
	}
}
