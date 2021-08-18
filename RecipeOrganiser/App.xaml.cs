using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Repositories;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Utils;
using RecipeOrganiser.ViewModels;

namespace RecipeOrganiser
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly IHost _host;

		public App()
		{
			_host = new HostBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					services.AddSingleton<IMapper, Mapper>();

					services.AddDbContext<RecipeOrganiserDbContext>();

					services.AddScoped<IRecipeRepository, RecipeRepository>();
					services.AddScoped<ICategoryRepository, CategoryRepository>();
					services.AddScoped<IIngredientRepository, IngredientRepository>();
					services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
					services.AddScoped<IUnitOfMeasurementRepository, UnitOfMeasurementRepository>();
					services.AddScoped<IShoppingListRepository, ShoppingListRepository>();
					services.AddScoped<IShoppingListRecipeRepository, ShoppingListRecipeRepository>();

					services.AddScoped<RecipeViewModel>();
					services.AddScoped<AddIngredientViewModel>();
					services.AddScoped<CategoriesViewModel>();
					services.AddScoped<ShoppingListViewModel>();
					services.AddScoped<EditShoppingListViewModel>();
					services.AddScoped<HomeViewModel>();

					services.AddSingleton<ApplicationViewModel>();
					services.AddSingleton<MainWindow>();

				}).Build();
		}

		private async void Application_Startup(object sender, StartupEventArgs e)
		{
			await _host.StartAsync();
			var mainWindow = _host.Services.GetService<MainWindow>();
			mainWindow.Show();
		}

		private async void Application_Exit(object sender, ExitEventArgs e)
		{
			using (_host)
			{
				await _host.StopAsync(TimeSpan.FromSeconds(5));
			}
		}
	}
}
