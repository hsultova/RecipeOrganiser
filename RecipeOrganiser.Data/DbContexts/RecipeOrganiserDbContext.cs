using Microsoft.EntityFrameworkCore;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Data.DbContexts
{
	public class RecipeOrganiserDbContext : DbContext
	{
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Database=RecipeOrganiser");
			base.OnConfiguring(optionsBuilder);
		}
	}
}
