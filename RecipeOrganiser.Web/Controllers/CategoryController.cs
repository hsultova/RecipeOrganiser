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
			var categories = _categoryService.GetAll();
			var model = new List<CategoryViewModel>();
			foreach (var category in categories)
			{
					model.Add(new CategoryViewModel
					{
						Id = category.Id,
						Name = category.Name,
						Description = category.Description,
						RecipeCount = _categoryService.GetRecipeCount(category.Id)
					});
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			var model = new CategoryViewModel();
			return View(model);
		}

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

		public IActionResult Edit(int id)
		{
			var category = _categoryService.Get(id);
			var model = new CategoryViewModel
			{
				Id = id,
				Name = category.Name,
				Description = category.Description,
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, CategoryViewModel model)
		{
			if (ModelState.IsValid)
			{
				//ToDO: Edit RecipeIngredients
				_categoryService.Update(id, model.Name, model.Description);

				return RedirectToAction(nameof(Index));

			}

			return View(model);
		}

		// GET: RecipeController/Delete/5
		public IActionResult Delete(int id)
		{
			_categoryService.Delete(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
