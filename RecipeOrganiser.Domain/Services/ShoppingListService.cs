using System.Collections.Generic;
using System.Linq;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Domain.Services.Abstract;

namespace RecipeOrganiser.Domain.Services
{
	public class ShoppingListService : IShoppingListService
	{
		private readonly IRecipeRepository _recipeRepository;
		private readonly IShoppingListRepository _shoppingListRepository;
		public ShoppingListService(IRecipeRepository recipeRepository,
			IShoppingListRepository shoppingListRepository)
		{
			_recipeRepository = recipeRepository;
			_shoppingListRepository = shoppingListRepository;
		}

		public IList<ShoppingList> GetAll()
		{
			return _shoppingListRepository.GetAll();
		}

		public IList<ShoppingList> GetAllWithIngredients()
		{
			return _shoppingListRepository.GetAll(null, x => x.ShoppingListIngredients);
		}

		public ShoppingList GetWithIngredients(int id)
		{
			return _shoppingListRepository.Get(r => r.Id == id, r => r.ShoppingListIngredients);
		}

		public int Create(string name, string description)
		{
			var list = new ShoppingList { Name = name, Description = description };
			_shoppingListRepository.Create(list);
			_shoppingListRepository.SaveChanges();

			return list.Id;
		}

		public void Update(int id, string name, string description)
		{
			var list = _shoppingListRepository.Get(l => l.Id == id);
			list.Name = name;
			list.Description = description;

			_shoppingListRepository.Update(list);
			_shoppingListRepository.SaveChanges();
		}

		public void Delete(int id)
		{
			_shoppingListRepository.Delete(id);
			_shoppingListRepository.SaveChanges();
		}

		public ShoppingList AddRecipeToShoppingList(int recipeId)
		{
			var shoppingList = new ShoppingList { Name = "Untitled" };

			var recipe = _recipeRepository.Get(x => x.Id == recipeId, x => x.RecipeIngredients);
			var shoppingListRecipe = new ShoppingListRecipe
			{
				Recipe = recipe,
				ShoppingList = shoppingList
			};
			shoppingList.ShoppingListRecipes.Add(shoppingListRecipe);

			foreach (var recipeIngredient in recipe.RecipeIngredients)
			{
				var shoppingListIngredient = new ShoppingListIngredient
				{
					Ingredient = recipeIngredient.Ingredient,
					Quantity = recipeIngredient.Quantity,
					UnitOfMeasurement = recipeIngredient.UnitOfMeasurement,
					Weight = recipeIngredient.Weight
				};

				shoppingList.ShoppingListIngredients.Add(shoppingListIngredient);
			}

			_shoppingListRepository.Create(shoppingList);
			_shoppingListRepository.SaveChanges();

			return shoppingList;
		}

		public void AddRecipeToShoppingList(int shoppingListId, int recipeId)
		{
			var shoppingList = _shoppingListRepository.Get(x => x.Id == shoppingListId, x => x.ShoppingListRecipes, x => x.ShoppingListIngredients);

			var recipe = _recipeRepository.Get(x => x.Id == recipeId, x => x.RecipeIngredients);

			_recipeRepository.Get(r => r.Id == recipe.Id, r => r.RecipeIngredients);
			var shoppingListRecipe = new ShoppingListRecipe
			{
				Recipe = recipe,
				ShoppingList = shoppingList
			};
			shoppingList.ShoppingListRecipes.Add(shoppingListRecipe);

			foreach (var recipeIngredient in recipe.RecipeIngredients)
			{
				var shoppingListIngredient = shoppingList.ShoppingListIngredients.FirstOrDefault(s => s.IngredientId == recipeIngredient.IngredientId);
				if (shoppingListIngredient == null)
				{
					shoppingListIngredient = new ShoppingListIngredient
					{
						Ingredient = recipeIngredient.Ingredient,
						Quantity = recipeIngredient.Quantity,
						UnitOfMeasurement = recipeIngredient.UnitOfMeasurement,
						Weight = recipeIngredient.Weight
					};
					shoppingList.ShoppingListIngredients.Add(shoppingListIngredient);
					continue;
				}

				shoppingListIngredient.Quantity += recipeIngredient.Quantity;
				shoppingListIngredient.Weight += recipeIngredient.Weight;
			}

			_shoppingListRepository.Update(shoppingList);
			_shoppingListRepository.SaveChanges();
		}

		public ShoppingList AddRecipesToShoppingList(IEnumerable<Recipe> recipes)
		{
			var shoppingList = new ShoppingList { Name = "Untitled" };

			shoppingList.ShoppingListRecipes = new List<ShoppingListRecipe>();
			shoppingList.ShoppingListIngredients = new List<ShoppingListIngredient>();

			foreach (var recipe in recipes)
			{
				_recipeRepository.Get(x => x.Id == recipe.Id, x => x.RecipeIngredients);
				var shoppingListRecipe = new ShoppingListRecipe
				{
					Recipe = recipe,
					ShoppingList = shoppingList
				};
				shoppingList.ShoppingListRecipes.Add(shoppingListRecipe);

				foreach (var recipeIngredient in recipe.RecipeIngredients)
				{
					var shoppingListIngredient = new ShoppingListIngredient
					{
						Ingredient = recipeIngredient.Ingredient,
						Quantity = recipeIngredient.Quantity,
						UnitOfMeasurement = recipeIngredient.UnitOfMeasurement,
						Weight = recipeIngredient.Weight
					};

					shoppingList.ShoppingListIngredients.Add(shoppingListIngredient);
				}
			}

			_shoppingListRepository.Create(shoppingList);
			_shoppingListRepository.SaveChanges();

			return shoppingList;
		}

		public void AddRecipesToShoppingList(int shoppingListId, IEnumerable<Recipe> recipes)
		{
			var shoppingList = _shoppingListRepository.Get(x => x.Id == shoppingListId, x => x.ShoppingListRecipes, x => x.ShoppingListIngredients);

			foreach (var recipe in recipes)
			{
				_recipeRepository.Get(r => r.Id == recipe.Id, r => r.RecipeIngredients);
				var shoppingListRecipe = new ShoppingListRecipe
				{
					Recipe = recipe,
					ShoppingList = shoppingList
				};
				shoppingList.ShoppingListRecipes.Add(shoppingListRecipe);
			}

			List<RecipeIngredient> allRecipeIngredients = recipes.SelectMany(r => r.RecipeIngredients).ToList();
			foreach (var recipeIngredient in allRecipeIngredients)
			{
				var shoppingListIngredient = shoppingList.ShoppingListIngredients.FirstOrDefault(s => s.IngredientId == recipeIngredient.IngredientId);
				if (shoppingListIngredient == null)
				{
					shoppingListIngredient = new ShoppingListIngredient
					{
						Ingredient = recipeIngredient.Ingredient,
						Quantity = recipeIngredient.Quantity,
						UnitOfMeasurement = recipeIngredient.UnitOfMeasurement,
						Weight = recipeIngredient.Weight
					};
					shoppingList.ShoppingListIngredients.Add(shoppingListIngredient);
					continue;
				}

				shoppingListIngredient.Quantity += recipeIngredient.Quantity;
				shoppingListIngredient.Weight += recipeIngredient.Weight;
			}

			_shoppingListRepository.Update(shoppingList);
			_shoppingListRepository.SaveChanges();
		}

		public void Reload(ShoppingList shoppingList)
		{
			_shoppingListRepository.Reload(shoppingList);
		}
	}
}
