using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Repositories;
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
					services.AddScoped<IMapper, Mapper>();

					services.AddDbContext<RecipeOrganiserDbContext>();
					services.AddScoped<IRecipeRepository, RecipeRepository>();
					services.AddScoped<ICategoryRepository, CategoryRepository>();
					services.AddScoped<IIngredientRepository, IngredientRepository>();
					services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
					services.AddScoped<IUnitOfMeasurementRepository, UnitOfMeasurementRepository>();

					services.AddSingleton<ApplicationViewModel>();
					services.AddSingleton<NewRecipeViewModel>();
					services.AddSingleton<AddIngredientViewModel>();
					services.AddSingleton<HomeViewModel>();
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
