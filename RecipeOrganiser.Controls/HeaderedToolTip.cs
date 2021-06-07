using System.Windows;
using System.Windows.Controls;

namespace RecipeOrganiser.Controls
{
	/// <summary>
	/// Custom control of the ToolTip with header and content text.
	/// </summary>
	public class HeaderedToolTip : ToolTip
	{
		static HeaderedToolTip()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedToolTip), new FrameworkPropertyMetadata(typeof(HeaderedToolTip)));
		}

		/// <summary>
		/// Text of the header.
		/// </summary>
		public string HeaderText
		{
			get { return (string)GetValue(HeaderTextProperty); }
			set { SetValue(HeaderTextProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="HeaderText"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderTextProperty =
			DependencyProperty.Register("HeaderText", typeof(string), typeof(HeaderedToolTip), new PropertyMetadata("Header"));

		/// <summary>
		/// Text of the content. Desription of the tool tip.
		/// </summary>
		public string ContentText
		{
			get { return (string)GetValue(ContentTextProperty); }
			set { SetValue(ContentTextProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="ContentText"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty ContentTextProperty =
			DependencyProperty.Register("ContentText", typeof(string), typeof(HeaderedToolTip), new PropertyMetadata(string.Empty));
	}
}
