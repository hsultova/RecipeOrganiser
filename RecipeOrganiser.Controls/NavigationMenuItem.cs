using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RecipeOrganiser.Controls
{
	/// <summary>
	/// Represents an item used in the navigation menu custom control.
	/// </summary>
	public class NavigationMenuItem : ListBoxItem
	{
		static NavigationMenuItem()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationMenuItem), new FrameworkPropertyMetadata(typeof(NavigationMenuItem)));
		}

		/// <summary>
		/// ImageSource (Image path) for the menu item icon. The default is null.
		/// </summary>
		public ImageSource Icon
		{
			get { return (ImageSource)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="Icon"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IconProperty =
			DependencyProperty.Register("Icon", typeof(ImageSource), typeof(NavigationMenuItem), new PropertyMetadata(null));

		/// <summary>
		/// Text of the the menu item element.
		/// </summary>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="Text"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(NavigationMenuItem), new PropertyMetadata(string.Empty));

		/// <summary>
		/// The command to execute when a menu item is selected.
		/// </summary>
		public ICommand SelectionCommand
		{
			get { return (ICommand)GetValue(SelectionCommandProperty); }
			set { SetValue(SelectionCommandProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SelectionCommand"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SelectionCommandProperty =
			DependencyProperty.Register("SelectionCommand", typeof(ICommand), typeof(NavigationMenuItem), new PropertyMetadata(null));
	}
}
