namespace RecipeOrganiser.Utils
{
	/// <summary>
	/// Represents an object to object mapper.
	/// Can be used when needed object mapping between layers, for example mapping between models and view models.
	/// </summary>
	public interface IMapper
	{
		/// <summary>
		/// Executes a mapping from source object to destination object.
		/// </summary>
		/// <param name="source">Source object to map from.</param>
		/// <param name="destination">Destination object to map into.</param>
		void Map(object source, object destination);

		/// <summary>
		///  Executes a mapping from source object to destination object. Can be specified properties to be ignored and not to be mapped.
		/// </summary>
		/// <param name="source">Source object to map from.</param>
		/// <param name="destination">Destination object to map into.</param>
		/// <param name="propertyNamesToIgonore">Name of the property to be ignored.</param>
		void Map(object source, object destination, params string[] propertyNamesToIgonore);
	}
}
