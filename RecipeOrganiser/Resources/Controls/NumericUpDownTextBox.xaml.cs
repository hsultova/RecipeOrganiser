using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecipeOrganiser.Resources.Controls
{
	/// <summary>
	/// Interaction logic for NumericUpDownTextBox.xaml
	/// </summary>
	public partial class NumericUpDownTextBox : UserControl
	{
		/// <summary>
		/// Describes the format type of the numeric textbox. The default is Int.
		/// </summary>
		public enum FormatType
		{
			Int,
			Decimal
		}

		public NumericUpDownTextBox()
		{
			InitializeComponent();
		}

		#region Dependency Properties

		#region Value

		/// <summary>
		/// A numeric value of the textbox
		/// </summary>
		public decimal Value
		{
			get { return (decimal)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="Value" dependency property/>
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(decimal), typeof(NumericUpDownTextBox), new FrameworkPropertyMetadata(0.0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnValuePropertyChanged)));

		/// <summary>
		/// Handles changes to Value property.
		/// </summary>
		/// <param name="d">NumericUpDownTextBox</param>
		/// <param name="e">DependencyProperty changed event arguments</param>
		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var numericBox = (NumericUpDownTextBox)d;
			string text = string.Empty;
			if (numericBox.ValueType == FormatType.Int)
			{
				int newValue = Convert.ToInt32(e.NewValue);
				text = newValue.ToString();
			}
			else
			{
				text = e.NewValue.ToString();
			}
			numericBox.NumericTextBox.Text = text;
		}
		#endregion

		#region Minimum

		/// <summary>
		/// Minimum value of the numeric textbox. The default is 0.
		/// </summary>
		public decimal Minimum
		{
			get { return (decimal)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="Minimum" dependency property/>
		/// </summary>
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(decimal), typeof(NumericUpDownTextBox), new PropertyMetadata(0.0m));

		#endregion

		/// <summary>
		/// Maximum value of the numeric textbox. The default is Decimal.MaxValue.
		/// </summary>
		#region Maximum
		public decimal Maximum
		{
			get { return (decimal)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="Maximum" dependency property/>
		/// </summary>
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(decimal), typeof(NumericUpDownTextBox), new PropertyMetadata(decimal.MaxValue));
		#endregion

		#region Increment
		/// <summary>
		/// Incrementing value. The default is 1.
		/// </summary>
		public decimal Increment
		{
			get { return (decimal)GetValue(IncrementProperty); }
			set { SetValue(IncrementProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="Increment" dependency property/>
		/// </summary>
		public static readonly DependencyProperty IncrementProperty =
			DependencyProperty.Register("Increment", typeof(decimal), typeof(NumericUpDownTextBox), new PropertyMetadata(1.0m));
		#endregion

		#region ValueType

		/// <summary>
		/// Format type of the value. The default is Int.
		/// </summary>
		public FormatType ValueType
		{
			get { return (FormatType)GetValue(ValueTypeProperty); }
			set { SetValue(ValueTypeProperty, value); }
		}

		/// <summary>
		/// Identifies <see cref="ValueType" dependency property/>
		/// </summary>
		public static readonly DependencyProperty ValueTypeProperty =
			DependencyProperty.Register("ValueType", typeof(FormatType), typeof(NumericUpDownTextBox), new PropertyMetadata(FormatType.Int, new PropertyChangedCallback(OnValueTypePropertyChanged)));

		/// <summary>
		/// Handles changes to ValueType property.
		/// </summary>
		/// <param name="d">NumericUpDownTextBox</param>
		/// <param name="e">DependencyProperty changed event arguments</param>
		private static void OnValueTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var numericTextBox = (NumericUpDownTextBox)d;
			if (numericTextBox.ValueType == FormatType.Decimal)
			{
				numericTextBox.Increment = 0.1m;
			}
		}
		#endregion
		#endregion

		#region RoutedEvents

		private static readonly RoutedEvent ValueChangedEvent =
			EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDownTextBox));

		/// <summary>
		/// Called when the value of the textbox changes.
		/// </summary>
		public event RoutedEventHandler ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		private static readonly RoutedEvent IncreaseClickedEvent =
		  EventManager.RegisterRoutedEvent("IncreaseClicked", RoutingStrategy.Bubble,
		  typeof(RoutedEventHandler), typeof(NumericUpDownTextBox));

		/// <summary>
		/// Called when increase button is clicked.
		/// </summary>
		public event RoutedEventHandler IncreaseClicked
		{
			add { AddHandler(IncreaseClickedEvent, value); }
			remove { RemoveHandler(IncreaseClickedEvent, value); }
		}

		private static readonly RoutedEvent DecreaseClickedEvent =
			EventManager.RegisterRoutedEvent("DescreaseClicked", RoutingStrategy.Bubble,
				typeof(RoutedEventHandler), typeof(NumericUpDownTextBox));

		/// <summary>
		/// Called when decrease button is clicked.
		/// </summary>
		public event RoutedEventHandler DecreaseClicked
		{
			add { AddHandler(DecreaseClickedEvent, value); }
			remove { RemoveHandler(DecreaseClickedEvent, value); }
		}

		#endregion

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			string matchNumbersRegex = @"^[0-9]*(?:\.[0-9]*)?$";
			var regex = new Regex(matchNumbersRegex);
			var match = regex.Match(e.Text);
			if (!match.Success)
			{
				e.Handled = true;
			}
		}

		private void IncreaseButton_Click(object sender, RoutedEventArgs e)
		{
			if (Value.CompareTo(Maximum) < 0)
			{
				Value += Increment;
				RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
			}
		}

		private void DecreaseButton_Click(object sender, RoutedEventArgs e)
		{
			if (Value.CompareTo(Minimum) > 0)
			{
				Value -= Increment;
				RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
			}
		}

		private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = (TextBox)sender;

			if (ValueType == FormatType.Int)
			{
				if (!Int32.TryParse(textBox.Text, out int result))
				{
					return;
				}
				Value = result;
			}
			else if (ValueType == FormatType.Decimal)
			{
				if (!Decimal.TryParse(textBox.Text, out decimal result))
				{
					return;
				}
				Value = result;
			}

			if (Value < Minimum)
				Value = Minimum;
			if (Value > Maximum)
				Value = Maximum;

			RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
		}
	}
}
