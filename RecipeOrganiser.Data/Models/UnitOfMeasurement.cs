namespace RecipeOrganiser.Data.Models
{
	/// <summary>
	/// Used for storing units of measurement, for example g, kg...
	/// </summary>
	public class UnitOfMeasurement : BaseModel
	{
		/// <summary>
		/// Name of the unit
		/// </summary>
		public string Name { get; set; }
	}
}
