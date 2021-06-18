using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RecipeOrganiser.Utils
{
	/// <summary>
	/// Represents a pair of corresponding property source and property destination.
	/// </summary>
	public class PropertyPair
	{
		public PropertyPair(PropertyInfo sourceProperty, PropertyInfo destinationProperty)
		{
			SourceProperty = sourceProperty;
			DestinationProperty = destinationProperty;
		}

		public PropertyInfo SourceProperty { get; }
		public PropertyInfo DestinationProperty { get; }
	}

	///<inheritdoc/>
	public sealed class Mapper : IMapper
	{
		private readonly Dictionary<string, PropertyPair[]> _mappings;

		public Mapper()
		{
			_mappings = new Dictionary<string, PropertyPair[]>();
		}

		///<inheritdoc/>
		///Property names of the source and destination should match.
		public void Map(object source, object destination)
		{
			Type sourceType = source.GetType();
			Type destinationType = destination.GetType();

			string key = GetKey(sourceType, destinationType);
			if (!_mappings.ContainsKey(key))
			{
				MapTypes(sourceType, destinationType);
			}

			PropertyPair[] propertyPairs = _mappings[key];
			foreach (PropertyPair pair in propertyPairs)
			{
				var sourceValue = pair.SourceProperty.GetValue(source);
				pair.DestinationProperty.SetValue(destination, sourceValue);
			}
		}

		///<inheritdoc/>
		///Property names of the source and destination should match.
		public void Map(object source, object destination, params string[] propertyNamesToIgonore)
		{
			Type sourceType = source.GetType();
			Type destinationType = destination.GetType();

			string key = GetKey(sourceType, destinationType);
			if (!_mappings.ContainsKey(key))
			{
				MapTypes(sourceType, destinationType);
			}

			PropertyPair[] propertyPairs = _mappings[key];
			foreach (PropertyPair pair in propertyPairs)
			{
				if (propertyNamesToIgonore.Contains(pair.SourceProperty.Name))
					continue;
				var sourceValue = pair.SourceProperty.GetValue(source);
				pair.DestinationProperty.SetValue(destination, sourceValue);
			}
		}


		private void MapTypes(Type sourceType, Type destinationType)
		{
			PropertyInfo[] sourceProperties = sourceType.GetProperties();
			PropertyInfo[] destinationProperties = destinationType.GetProperties();

			var propertyPairs = new List<PropertyPair>();
			foreach (PropertyInfo destinationProperty in destinationProperties)
			{
				if (destinationProperty.CanWrite)
				{
					PropertyInfo sourceProperty = sourceProperties.FirstOrDefault(p => p.Name == destinationProperty.Name);
					if (sourceProperty == null)
					{
						continue;
					}

					propertyPairs.Add(new PropertyPair(sourceProperty, destinationProperty));
				}
			}

			string key = GetKey(sourceType, destinationType);
			_mappings.Add(key, propertyPairs.ToArray());
		}

		private string GetKey(Type sourceType, Type destinationType)
		{
			return $"{sourceType}_{destinationType}";
		}
	}
}
