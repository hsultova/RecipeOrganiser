using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using RecipeOrganiser.Utils.Events;

namespace RecipeOrganiser.Utils.Converters
{
	public class MessageTypeToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DisplayMessageType type && value != null)
			{
				var color = type switch
				{
					DisplayMessageType.Info => new SolidColorBrush(Colors.LightSteelBlue),
					DisplayMessageType.Error => new SolidColorBrush(Colors.PaleVioletRed),
					DisplayMessageType.None => new SolidColorBrush(Colors.White),
					_ => new SolidColorBrush(Colors.LightGray),
				};

				return color;
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}