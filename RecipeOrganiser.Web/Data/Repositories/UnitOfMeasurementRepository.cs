using RecipeOrganiser.Domain.Models;
using RecipeOrganiser.Domain.Repositories;
using RecipeOrganiser.Web.Data;

namespace RecipeOrganiser.Data.Repositories
{
	public class UnitOfMeasurementRepository : Repository<UnitOfMeasurement>, IUnitOfMeasurementRepository
	{
		public UnitOfMeasurementRepository(ApplicationDbContext context)
			: base(context)
		{
		}
	}
}
