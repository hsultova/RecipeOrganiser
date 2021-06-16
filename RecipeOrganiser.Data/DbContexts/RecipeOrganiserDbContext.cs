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
		public DbSet<ShoppingList> ShoppingLists { get; set; }

		public DbSet<ShoppingListRecipe> ShoppingListRecipes { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//To be moved in config file
			optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Database=RecipeOrganiser");
			base.OnConfiguring(optionsBuilder);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UnitOfMeasurement>()
				.Property(p => p.Name)
				.HasDefaultValue("None")
				.IsRequired(true);

			modelBuilder.Entity<RecipeIngredient>()
				.Property(p => p.UnitOfMeasurementId)
				.HasDefaultValue(1);

			modelBuilder.Entity<Recipe>()
				.Property(p => p.Name)
				.IsRequired(true);

			modelBuilder.Entity<Ingredient>()
				.Property(p => p.Name)
				.IsRequired(true);

			modelBuilder.Entity<Category>()
				.Property(p => p.Name)
				.IsRequired(true);

			//Data Seed
			modelBuilder.Entity<UnitOfMeasurement>().HasData(
				new UnitOfMeasurement { Id = 1, Name = "None", ShortName = "None" },
				new UnitOfMeasurement { Id = 2, Name = "Kilogram (kg)", ShortName = "kg" },
				new UnitOfMeasurement { Id = 3, Name = "Gram (g)", ShortName = "g" },
				new UnitOfMeasurement { Id = 4, Name = "Milligrams (mg)", ShortName = "mg" },
				new UnitOfMeasurement { Id = 5, Name = "Litre (L)", ShortName = "L" },
				new UnitOfMeasurement { Id = 6, Name = "Millilitre (L)", ShortName = "mL" });

			base.OnModelCreating(modelBuilder);
		}
	}
}
