using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeOrganiser.Web.Models
{
	public class RecipesIndexViewModel
	{
		public IEnumerable<RecipeViewModel> Recipes { get; set; }
		public IEnumerable<SelectListItem> Categories { get; set; }
		public IEnumerable<SelectListItem> Ingredients { get; set; }

		public string  SearchText { get; set; }
		public string CategoryName { get; set; }
		public string IngredientName { get; set; }
	}
}
