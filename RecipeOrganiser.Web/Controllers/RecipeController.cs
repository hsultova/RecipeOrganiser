using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Services.Abstract;
using RecipeOrganiser.Web.Helpers;
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
		public IActionResult Index()
		{
			var model = new List<RecipeViewModel>();
			IList<Recipe> recipes = _recipeService.GetAll();

			foreach (var recipe in recipes)
			{
				model.Add(new RecipeViewModel
				{
					Id = recipe.Id,
					Name = recipe.Name,
					Description = recipe.Description,
					Note = recipe.Note,
					CategoryId = recipe.CategoryId
				});
			}

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
			var model = new RecipeViewModel { Categories = _categoryService.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = true }) };
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

			model.Categories = _categoryService.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = true });
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
