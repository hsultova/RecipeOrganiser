using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace RecipeOrganiser.Controls
{
	/// <summary>
	/// Reperesents statest of the search mode of the text box.
	/// </summary>
	public enum SearchMode
	{
		/// <summary>
		/// Executes the search while typing. Can be set time interval.
		/// </summary>
		Instant,
		/// <summary>
		/// Executes the search pressing enter key or search button.
		/// </summary>
		Delayed,
	}

	/// <summary>
	/// Represents custom control of the search text box.
	/// </summary>
	public class SearchTextBox : TextBox
	{
		private DispatcherTimer _searchEventDelayTimer;

		static SearchTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchTextBox), new FrameworkPropertyMetadata(typeof(SearchTextBox)));
		}

		public SearchTextBox() : base()
		{
			_searchEventDelayTimer = new DispatcherTimer();
			_searchEventDelayTimer.Interval = SearchEventTimeDelay.TimeSpan;
			_searchEventDelayTimer.Tick += new EventHandler(OnSeachEventDelayTimerTick);
		}

		#region Dependency properties

		/// <summary>
		/// Label text showed before entering text. The default is "Search".
		/// </summary>
		public string LabelText
		{
			get { return (string)GetValue(LabelTextProperty); }
			set { SetValue(LabelTextProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="LabelText"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty LabelTextProperty =
			DependencyProperty.Register("LabelText", typeof(string), typeof(SearchTextBox), new PropertyMetadata("Search"));

		/// <summary>
		/// The color of the label text.
		/// </summary>
		public Brush LabelTextColor
		{
			get { return (Brush)GetValue(LabelTextColorProperty); }
			set { SetValue(LabelTextColorProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="LabelTextColor"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty LabelTextColorProperty =
			DependencyProperty.Register("LabelTextColor", typeof(Brush), typeof(SearchTextBox), new PropertyMetadata(Brushes.Black));

		/// <summary>
		/// Mode of the search.
		/// </summary>
		public SearchMode SearchMode
		{
			get { return (SearchMode)GetValue(SearchModeProperty); }
			set { SetValue(SearchModeProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SearchMode"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SearchModeProperty =
			DependencyProperty.Register("SearchMode", typeof(SearchMode), typeof(SearchTextBox), new PropertyMetadata(SearchMode.Instant));

		/// <summary>
		/// Indicates if the textbox has text.
		/// </summary>
		public bool HasText
		{
			get { return (bool)GetValue(HasTextProperty); }
			set { SetValue(HasTextProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="HasText"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty HasTextProperty =
			DependencyProperty.Register("HasText", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false));

		/// <summary>
		/// Background color on MouseDown.
		/// </summary>
		public Brush SearchIconBackgroundMouseDownColor
		{
			get { return (Brush)GetValue(SearchIconBackgroundMouseDownColorProperty); }
			set { SetValue(SearchIconBackgroundMouseDownColorProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SearchIconBackgroundMouseDownColorProperty"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SearchIconBackgroundMouseDownColorProperty =
			DependencyProperty.Register("SearchIconBackgroundMouseDownColor", typeof(Brush), typeof(SearchTextBox), new PropertyMetadata(Brushes.LightBlue));

		/// <summary>
		/// Border color of the search icon.
		/// </summary>
		public Brush SearchIconBorderColor
		{
			get { return (Brush)GetValue(SearchIconBorderColorProperty); }
			set { SetValue(SearchIconBorderColorProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SearchIconBorderColor"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SearchIconBorderColorProperty =
			DependencyProperty.Register("SearchIconBorderColor", typeof(Brush), typeof(SearchTextBox), new PropertyMetadata(Brushes.White));

		/// <summary>
		/// Property key for IsMouseLeftButtonDown
		/// </summary>
		private static DependencyPropertyKey IsMouseLeftButtonDownPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"IsMouseLeftButtonDown",
				typeof(bool),
				typeof(SearchTextBox),
				new PropertyMetadata());

		/// <summary>
		/// Dependency property of the IsMouseLeftButtonDownPropertyKey
		/// </summary>
		public static DependencyProperty IsMouseLeftButtonDownProperty =
			IsMouseLeftButtonDownPropertyKey.DependencyProperty;

		/// <summary>
		/// Indicates if the mouse left button is pressed
		/// </summary>
		public bool IsMouseLeftButtonDown
		{
			get { return (bool)GetValue(IsMouseLeftButtonDownProperty); }
			private set { SetValue(IsMouseLeftButtonDownPropertyKey, value); }
		}

		/// <summary>
		/// The command to execute when searching.
		/// </summary>
		public ICommand SearchCommand
		{
			get { return (ICommand)GetValue(SearchCommandProperty); }
			set { SetValue(SearchCommandProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SearchCommand"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SearchCommandProperty =
			DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchTextBox), new FrameworkPropertyMetadata(null));

		/// <summary>
		/// Duration of the search to trigger. The default is 0.5 seconds.
		/// </summary>
		public Duration SearchEventTimeDelay
		{
			get { return (Duration)GetValue(SearchEventTimeDelayProperty); }
			set { SetValue(SearchEventTimeDelayProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="SearchEventTimeDelay"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SearchEventTimeDelayProperty =
			DependencyProperty.Register("SearchEventTimeDelay", typeof(Duration), typeof(SearchTextBox), new FrameworkPropertyMetadata(
			new Duration(new TimeSpan(0, 0, 0, 0, 500)),
			new PropertyChangedCallback(OnSearchEventTimeDelayChanged)));

		#endregion

		#region Routed Events

		public static readonly RoutedEvent SearchEvent =
			EventManager.RegisterRoutedEvent("Search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchTextBox));

		/// <summary>
		/// Called when the search is applied.
		/// </summary>
		public event RoutedEventHandler Search
		{
			add { AddHandler(SearchEvent, value); }
			remove { RemoveHandler(SearchEvent, value); }
		}

		#endregion

		#region TextBoxBase
		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			base.OnTextChanged(e);
			HasText = Text.Length != 0;

			if (SearchMode == SearchMode.Instant)
			{
				_searchEventDelayTimer.Stop();
				_searchEventDelayTimer.Start();
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			Border iconBorder = GetTemplateChild("PART_SearchIconBorder") as Border;
			if (iconBorder == null)
			{
				return;
			}

			iconBorder.MouseLeftButtonDown += new MouseButtonEventHandler(IconBorder_MouseLeftButtonDown);
			iconBorder.MouseLeftButtonUp += new MouseButtonEventHandler(IconBorder_MouseLeftButtonUp);
			iconBorder.MouseLeave += new MouseEventHandler(IconBorder_MouseLeave);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (SearchMode == SearchMode.Instant && e.Key == Key.Escape)
			{
				this.Text = string.Empty;
			}
			else if (SearchMode == SearchMode.Delayed && (e.Key == Key.Return || e.Key == Key.Enter))
			{
				OnSearch();
			}
			else
			{
				base.OnKeyDown(e);
			}
		}

		#endregion

		private void IconBorder_MouseLeftButtonDown(object obj, MouseButtonEventArgs e)
		{
			IsMouseLeftButtonDown = true;
		}

		private void IconBorder_MouseLeftButtonUp(object obj, MouseButtonEventArgs e)
		{
			if (!IsMouseLeftButtonDown)
			{
				return;
			}

			if (SearchMode == SearchMode.Instant && HasText)
			{
				Text = string.Empty;
			}
			else if (SearchMode == SearchMode.Delayed && HasText)
			{
				OnSearch();
			}

			IsMouseLeftButtonDown = false;
		}

		private void IconBorder_MouseLeave(object obj, MouseEventArgs e)
		{
			IsMouseLeftButtonDown = false;
		}


		private static void OnSearchEventTimeDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SearchTextBox searchTextBox = d as SearchTextBox;
			if (searchTextBox == null)
			{
				return;
			}

			searchTextBox._searchEventDelayTimer.Interval = ((Duration)e.NewValue).TimeSpan;
			searchTextBox._searchEventDelayTimer.Stop();
		}

		private void OnSearch()
		{
			RaiseEvent(new RoutedEventArgs(SearchEvent));

			if (SearchCommand.CanExecute(null))
			{
				SearchCommand.Execute(null);
			}
		}

		void OnSeachEventDelayTimerTick(object o, EventArgs e)
		{
			_searchEventDelayTimer.Stop();
			OnSearch();
		}
	}
}
