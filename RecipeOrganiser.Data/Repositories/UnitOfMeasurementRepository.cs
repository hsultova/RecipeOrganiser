using RecipeOrganiser.Data.DbContexts;
using RecipeOrganiser.Data.Models;

namespace RecipeOrganiser.Data.Repositories
{
	public class UnitOfMeasurementRepository : Repository<UnitOfMeasurement>, IUnitOfMeasurementRepository
	{
		public UnitOfMeasurementRepository(RecipeOrganiserDbContext context)
			: base(context)
		{
		}
	}
}
