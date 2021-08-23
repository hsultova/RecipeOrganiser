using System.Collections.Generic;
using System.Linq;
using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Domain.Services.Abstract;

namespace RecipeOrganiser.Domain.Services
{
	public class RecipeService : IRecipeService
	{
		private readonly IRecipeRepository _recipeRepository;
		private readonly IIngredientRepository _ingredientRepository;
		private readonly IUnitOfMeasurementRepository _unitRepository;
		public RecipeService(
			IRecipeRepository recipeRepository,
			IIngredientRepository ingredientRepository,
			IUnitOfMeasurementRepository unitRepository)
		{
			_recipeRepository = recipeRepository;
			_ingredientRepository = ingredientRepository;
			_unitRepository = unitRepository;
		}

		public IList<Recipe> GetAll()
		{
			return _recipeRepository.GetAll();
		}

		public Recipe GetWithIngredients(int id)
		{
			return _recipeRepository.Get(r => r.Id == id, r => r.RecipeIngredients);
		}

		public int Create(
			string name,
			string description,
			string note,
			byte[] image,
			int categoryId)
		{
			var recipe = new Recipe
			{
				Name = name,
				Description = description,
				Note = note,
				Image = image,
				CategoryId = categoryId
			};

			_recipeRepository.Create(recipe);
			_recipeRepository.SaveChanges();

			return recipe.Id;
		}

		public int Create(
			string name,
			string description,
			string note,
			byte[] image, 
			Category category)
		{
			var recipe = new Recipe
			{
				Name = name,
				Description = description,
				Note = note,
				Image = image,
				Category = category
			};

			_recipeRepository.Create(recipe);
			_recipeRepository.SaveChanges();

			return recipe.Id;
		}

		public void Update(
			int id,
			string name,
			string description,
			string note,
			byte[] image,
			int categoryId)
		{
			var recipe = _recipeRepository.Get(r => r.Id == id);

			recipe.Name = name;
			recipe.Description = description;
			recipe.Note = note;
			recipe.Image = image;
			recipe.CategoryId = categoryId;

			_recipeRepository.Update(recipe);
			_recipeRepository.SaveChanges();
		}

		public void Update(
			int id,
			string name,
			string description,
			string note,
			byte[] image,
			Category category)
		{
			var recipe = _recipeRepository.Get(r => r.Id == id);

			recipe.Name = name;
			recipe.Description = description;
			recipe.Note = note;
			recipe.Image = image;
			recipe.Category = category;

			_recipeRepository.Update(recipe);
			_recipeRepository.SaveChanges();
		}

		public void Delete(int id)
		{
			_recipeRepository.Delete(id);
			_recipeRepository.SaveChanges();
		}

		public void CreateRecipeIngredient(
			int recipeId,
			int quantity,
			double weight,
			int ingredientId,
			int UnitId)
		{
			var recipeIngredient = new RecipeIngredient
			{
				Quantity = quantity,
				Weight = weight,
				IngredientId = ingredientId,
				UnitOfMeasurementId = UnitId
			};

			var recipe = _recipeRepository.Get(r => r.Id == recipeId);
			recipe.RecipeIngredients.Add(recipeIngredient);

			_recipeRepository.Update(recipe);
			_recipeRepository.SaveChanges();
		}
		public void CreateRecipeIngredient(
			int recipeId,
			int quantity,
			double weight, 
			Ingredient ingredient,
			UnitOfMeasurement Unit)
		{
			var recipeIngredient = new RecipeIngredient
			{
				Quantity = quantity,
				Weight = weight,
				Ingredient = ingredient,
				UnitOfMeasurement = Unit
			};

			var recipe = _recipeRepository.Get(r => r.Id == recipeId, r=> r.RecipeIngredients);
			recipe.RecipeIngredients.Add(recipeIngredient);

			_recipeRepository.Update(recipe);
			_recipeRepository.SaveChanges();
		}

		public void CreateRecipeIngredient(
			Recipe recipe,
			int quantity,
			double weight,
			Ingredient ingredient,
			UnitOfMeasurement Unit)
		{
			var recipeIngredient = new RecipeIngredient
			{
				Quantity = quantity,
				Weight = weight,
				Ingredient = ingredient,
				UnitOfMeasurement = Unit
			};

			recipe.RecipeIngredients?.Add(recipeIngredient);

			_recipeRepository.Update(recipe);
			_recipeRepository.SaveChanges();
		}

		public IList<Ingredient> GetIngredients()
		{
			return _ingredientRepository.GetAll();
		}

		public IList<UnitOfMeasurement> GetUnits()
		{
			return _unitRepository.GetAll();
		}

		public bool IsVisible(int recipeId, string searchText, string category, string ingredient, bool isFilterEnabled)
		{
			if (string.IsNullOrEmpty(searchText) && !isFilterEnabled)
			{
				return true;
			}

			Recipe recipe = GetWithIngredients(recipeId);

			bool hasName = true;
			if (!string.IsNullOrEmpty(searchText))
			{
				hasName = recipe.Name.ToLower().Contains(searchText.ToLower());
			}

			if (isFilterEnabled)
			{
				bool hasCategory = true;
				bool hasIngredient = true;

				if (!string.IsNullOrEmpty(category))
				{
					hasCategory = recipe.Category.Name == category;
				}

				if (!string.IsNullOrEmpty(ingredient))
				{
					hasIngredient = recipe.RecipeIngredients.Select(i => i.Ingredient.Name).Contains(ingredient);
				}

				if (hasName && hasCategory && hasIngredient)
				{
					return true;
				}

				return false;
			}

			return hasName;
		}
	}
}
