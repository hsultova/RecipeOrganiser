using System.Collections.Generic;
using RecipeOrganiser.Domain.Models;

namespace RecipeOrganiser.Domain.Services.Abstract
{
	public interface IRecipeService
	{
		IList<Recipe> GetAll();

		Recipe Get(int id);

		Recipe GetWithIngredients(int id);

		int Create(
			string name,
			string description,
			string note,
			byte[] image,
			int categoryId);

		int Create(
			string name,
			string description,
			string note,
			byte[] image,
			Category category);

		void Update(
			int id,
			string name,
			string description,
			string note,
			byte[] image,
			int categoryId);

		void Update(
			int id,
			string name,
			string description,
			string note,
			byte[] image,
			Category category);

		void Delete(int id);

		void CreateRecipeIngredient(
			int recipeId,
			int quantity,
			double weight,
			int ingredientId,
			int UnitId);

		void CreateRecipeIngredient(
			int recipeId,
			int quantity,
			double weight,
			Ingredient ingredient,
			UnitOfMeasurement Unit);

		void CreateRecipeIngredient(
			Recipe recipe,
			int quantity,
			double weight,
			Ingredient ingredient,
			UnitOfMeasurement Unit);

		IList<Ingredient> GetIngredients();
		IList<UnitOfMeasurement> GetUnits();

		bool IsVisible(int recipeId, string searchText, string category, string ingredient, bool isFilterEnabled);
	}
}
