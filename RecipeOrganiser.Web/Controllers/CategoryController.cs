using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Services.Abstract;
using RecipeOrganiser.Web.Models;

namespace RecipeOrganiser.Web.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{
			var model = new CategoryViewModel();
			return View(model);
		}

		// POST: RecipeController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(CategoryViewModel model)
		{
			if (ModelState.IsValid)
			{
				//ToDO: Create RecipeIngredients
				_categoryService.Create(model.Name, model.Description);

				return RedirectToAction(nameof(Index));

			}

			return View(model);
		}
	}
}
