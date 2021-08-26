using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Services.Abstract;
using RecipeOrganiser.Web.Helpers;
using RecipeOrganiser.Web.Helpers.Extensions;
using RecipeOrganiser.Web.Models;

namespace RecipeOrganiser.Web.Controllers
{
	public class RecipeController : Controller
	{
		private readonly IRecipeService _recipeService;
		private readonly ICategoryService _categoryService;

		public RecipeController(IWebHostEnvironment env, IRecipeService recipeService, ICategoryService categoryService)
		{
			_recipeService = recipeService;
			_categoryService = categoryService;
		}

		// GET: RecipeController
		public IActionResult Index(RecipesIndexViewModel model)
		{
			var recipesVm = new List<RecipeViewModel>();
			var recipes = _recipeService.GetAll();
			foreach (var recipe in recipes)
			{
				if(_recipeService.IsVisible(recipe.Id, model.SearchText, model.CategoryName, model.IngredientName, true))
				{
					recipesVm.Add(new RecipeViewModel
					{
						Id = recipe.Id,
						Name = recipe.Name,
						Description = recipe.Description,
						Note = recipe.Note,
						CategoryId = recipe.CategoryId
					});
				}
			}

			model.Recipes = recipesVm;
			model.Categories = _categoryService.GetAll().ToSelectListItem<Category>(x => x.Name, x => x.Name);
			model.Ingredients = _recipeService.GetIngredients().ToSelectListItem<Ingredient>(x => x.Name, x => x.Name);

			return View(model);
		}

		[HttpGet]
		public IActionResult GetRecipeImageFile(int recipeId)
		{
			var recipe = _recipeService.Get(recipeId);

			if (recipe.Image == null)
			{
				var path = Path.Combine("/images/placeholder.png");
				return File(path, "image/png");
			}

			return GetFileFromBytes(recipe.Image);
		}

		public FileResult GetFileFromBytes(byte[] bytesIn)
		{
			return File(bytesIn, "image/png");
		}

		// GET: RecipeController/Details/5
		public IActionResult Details(int id)
		{
			return View();
		}

		// GET: RecipeController/Create
		[HttpGet]
		public IActionResult Create()
		{
			var model = new RecipeViewModel { Categories = _categoryService.GetAll().ToSelectListItem<Category>(x => x.Name, x => x.Id.ToString()) };
			return View(model);
		}

		// POST: RecipeController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(RecipeViewModel model)
		{
			if (ModelState.IsValid)
			{
				//ToDO: Create RecipeIngredients
				_recipeService.Create(model.Name, model.Description, model.Note, model.Image?.ToBytes(), model.CategoryId);

				return RedirectToAction(nameof(Index));

			}

			model.Categories = _categoryService.GetAll().ToSelectListItem<Category>(x => x.Name, x => x.Id.ToString());
			return View(model);
		}

		public IActionResult AddIngredient()
		{
			return PartialView("_AddIngredientPartial");
		}

		// GET: RecipeController/Edit/5
		public IActionResult Edit(int id)
		{
			return View();
		}

		// POST: RecipeController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: RecipeController/Delete/5
		public IActionResult Delete(int id)
		{
			return View();
		}

		// POST: RecipeController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
