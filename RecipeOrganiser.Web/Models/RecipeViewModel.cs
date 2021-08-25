using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeOrganiser.Web.Models
{
	public class RecipeViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
		public string Note { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile Image { get; set; }
		[Display(Name = "Category")]
		public int CategoryId { get; set; }
		public IEnumerable<SelectListItem> Categories { get; set; }

		public IEnumerable<RecipeIngredientViewModel> Ingredients { get; set; }
	}
}
