using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using RecipeOrganiser.Utils.Events;

namespace RecipeOrganiser.ViewModels.Base
{
	[AttributeUsage(AttributeTargets.Property)]
	public class RequiredAttribute : Attribute
	{
	}

	/// <summary>
	/// Base view model with basic funcionality.
	/// </summary>
	public class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
	{
		private PropertyInfo[] _properties;
		public BaseViewModel()
		{
			_properties = GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(RequiredAttribute))).ToArray();
		}

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		/// <summary>
		/// Sets the backing field property only if the value is different.
		/// </summary>
		/// <typeparam name="T">Type of the property</typeparam>
		/// <param name="backingField">Backing field property to be set.</param>
		/// <param name="value">The new value.</param>
		/// <param name="propertyName">Name of the property to be updated.</param>
		/// <returns>The result, true if the propery is set, otherwise false.</returns>
		protected bool SetBackingFieldProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(backingField, value))
			{
				return false;
			}

			backingField = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		#endregion

		#region IDataErrorInfo

		private string _error;
		public string Error
		{
			get => _error;
			set => SetBackingFieldProperty<string>(ref _error, value, nameof(Error));
		}

		public string this[string columnName]
		{
			get => OnValidate(columnName);
		}

		protected virtual string OnValidate(string columnName)
		{
			string result = string.Empty;
			foreach (var property in _properties)
			{
				if (columnName == property.Name)
				{
				if(string.IsNullOrWhiteSpace(property.GetValue(this) as string))
					{
						result = $"{property.Name} is required.";
					}
				}
			}
			
			if (string.IsNullOrEmpty(result))
			{
				Error = null;
			}
			else
			{
				Error = "Error";
			}

			return result;
		}
		#endregion

		#region DisplayMessage Event
		public event EventHandler<DisplayMessageEventArgs> DisplayMessageHandler;

		/// <summary>
		/// Raise DisplayMessage event which displays a message with corresponding type.
		/// </summary>
		/// <param name="args">DisplayMessage event arguments.</param>
		protected void OnDisplayMessage(DisplayMessageEventArgs args) => DisplayMessageHandler?.Invoke(this, args);

		/// <summary>
		/// Raise DisplayMessage event with Info type which notifies when a record is created in the DB table.
		/// </summary>
		/// <typeparam name="T">Type of the created record.</typeparam>
		protected void OnRecordCreated<T>(string name = "")
		{
			OnDisplayMessage(
				new DisplayMessageEventArgs
				{
					Message = $"{typeof(T).Name} {name} created successfully.",
					MessageТype = DisplayMessageType.Info
				});
		}

		/// <summary>
		/// Raise DisplayMessage event with Info type which notifies when a record is updated in the DB table.
		/// </summary>
		/// <typeparam name="T">Type of the updated record.</typeparam>
		protected void OnRecordUpdated<T>(string name = "")
		{
			OnDisplayMessage(
				new DisplayMessageEventArgs
				{
					Message = $"{typeof(T).Name} {name} updated successfully.",
					MessageТype = DisplayMessageType.Info
				});
		}

		/// <summary>
		/// Raise DisplayMessage event with Info type which notifies when a record is deleted from the DB table.
		/// </summary>
		/// <typeparam name="T">Type of the deleted record.</typeparam>
		protected void OnRecordDeleted<T>(string name = "")
		{
			OnDisplayMessage(
				new DisplayMessageEventArgs
				{
					Message = $"{typeof(T).Name} {name} deleted.",
					MessageТype = DisplayMessageType.Info
				});
		}

		#endregion

		#region ChangeViewModel Event

		public event EventHandler<ChangeViewModelEventArgs> ChangeViewModelHandler;

		/// <summary>
		/// Raise ChangeViewModel event.
		/// </summary>
		/// <param name="args">ChangeViewModel event arguments.</param>
		protected void OnChangeViewModel(ChangeViewModelEventArgs args) => ChangeViewModelHandler?.Invoke(this, args);

		#endregion

		/// <summary>
		/// Represents a state if the view model is ready to exit. 
		/// For example if there is unsaved changes, should be false. Default value is true.
		/// </summary>
		/// <returns>Value indicating if the view model is ready to exit.</returns>
		public bool CanExit { get; protected set; } = true;

		/// <summary>
		/// Sets all the fields to their default values in the view model.
		/// </summary>
		public virtual void Clear()
		{
		}

		/// <summary>
		/// Refreshes view model data.
		/// </summary>
		public virtual void Refresh()
		{
		}
	}
}
