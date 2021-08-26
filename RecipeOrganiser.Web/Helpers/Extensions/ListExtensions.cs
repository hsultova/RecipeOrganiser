using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeOrganiser.Web.Helpers.Extensions
{
	public static class ListExtensions
	{
		public static IList<SelectListItem> ToSelectListItem<T>(
			this IList<T> list,
			Func<T, string> text,
			Func<T, string> value,
			string selectedValue = null)
		{
			return list
				.Select(
				item => new SelectListItem
				{
					Value = value(item),
					Text = text(item),
					Selected = selectedValue == value(item) ? true : false
				})
				.OrderBy(x => x.Text)
				.ToList();
		}
	}
}
