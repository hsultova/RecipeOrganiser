using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RecipeOrganiser.Controls
{
	/// <summary>
	/// Represents a custom control for the navigation menu. Can be customized with different colors as background, 
	/// foregrond of the menu items, selection color, selection indicator color, etc...
	/// </summary>
	public class NavigationMenu : Control
	{
		static NavigationMenu()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationMenu), new FrameworkPropertyMetadata(typeof(NavigationMenu)));
		}

		public override void BeginInit()
		{
			Items = new List<NavigationMenuItem>();
			base.BeginInit();
		}

		/// <summary>
		/// Navigation menu items.
		/// </summary>
		public List<NavigationMenuItem> Items
		{
			get { return (List<NavigationMenuItem>)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="Items"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty ItemsProperty =
			DependencyProperty.Register("Items", typeof(List<NavigationMenuItem>), typeof(NavigationMenu),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>
		/// Indicates whether the control is expanded. The default is false.
		/// </summary>
		public bool IsExpanded
		{
			get { return (bool)GetValue(IsExpandedProperty); }
			set { SetValue(IsExpandedProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="IsExpanded"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsExpandedProperty =
			DependencyProperty.Register("IsExpanded", typeof(bool), typeof(NavigationMenu), new PropertyMetadata(false));

		/// <summary>
		/// A brush that describes the foreground of the menu items. The default is White.
		/// </summary>
		public Brush MenuItemForeground
		{
			get { return (Brush)GetValue(MenuItemForegroundProperty); }
			set { SetValue(MenuItemForegroundProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="MenuItemForeground"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty MenuItemForegroundProperty =
			DependencyProperty.Register("MenuItemForeground", typeof(Brush), typeof(NavigationMenu), new PropertyMetadata(Brushes.White));

		/// <summary>
		/// A brush to color the element on selection (on hover). The default is Gray.
		/// </summary>
		public Brush SelectionColor
		{
			get { return (Brush)GetValue(SelectionColorProperty); }
			set { SetValue(SelectionColorProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SelectionColor"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SelectionColorProperty =
			DependencyProperty.Register("SelectionColor", typeof(Brush), typeof(NavigationMenu), new PropertyMetadata(Brushes.Gray));

		/// <summary>
		/// A brush to color the icon of the button used for collapsing the menu. The default is White.
		/// </summary>
		public Brush ButtonIconColor
		{
			get { return (Brush)GetValue(ButtonIconColorProperty); }
			set { SetValue(ButtonIconColorProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="ButtonIconColor"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty ButtonIconColorProperty =
			DependencyProperty.Register("ButtonIconColor", typeof(Brush), typeof(NavigationMenu), new PropertyMetadata(Brushes.White));

		/// <summary>
		/// A brush that paint the indicator when an item is selected. The default is White.
		/// </summary>
		public Brush SelectionIndicatorColor
		{
			get { return (Brush)GetValue(SelectionIndicatorColorProperty); }
			set { SetValue(SelectionIndicatorColorProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SelectionIndicatorColor"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SelectionIndicatorColorProperty =
			DependencyProperty.Register("SelectionIndicatorColor", typeof(Brush), typeof(NavigationMenuItem), new PropertyMetadata(Brushes.White));

		/// <summary>
		/// Indicates the selected menu item.
		/// </summary>
		public NavigationMenuItem SelectedMenuItem
		{
			get { return (NavigationMenuItem)GetValue(SelectedMenuItemProperty); }
			set { SetValue(SelectedMenuItemProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SelectedMenuItem"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SelectedMenuItemProperty = DependencyProperty.Register("SelectedMenuItem",
			typeof(NavigationMenuItem), 
			typeof(NavigationMenu), 
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
	}
}
