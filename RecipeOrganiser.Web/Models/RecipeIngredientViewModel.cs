using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeOrganiser.Web.Models
{
	public class RecipeIngredientViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public int Quantity { get; set; }
		public int Weight { get; set; }
		public int RecipeId { get; set; }
		public int IngredientId { get; set; }
		public int UnitId { get; set; }
		public IEnumerable<SelectListItem> Ingredients { get; set; }
		public IEnumerable<SelectListItem> Units { get; set; }
	}
}
