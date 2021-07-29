using System.Collections.Generic;

namespace RecipeOrganiser.Domain.Models
{
	/// <summary>
	/// A category which contains recipes
	/// </summary>
	public class Category : BaseModel
	{
		/// <summary>
		/// Name of the category
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Description of the category
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// All recipes in the category
		/// </summary>
		public virtual ICollection<Recipe> Recipes { get; set; }
	}
}
