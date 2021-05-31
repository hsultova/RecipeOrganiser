using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RecipeOrganiser.Utils.Events;

namespace RecipeOrganiser.ViewModels.Base
{
	/// <summary>
	/// Base view model with basic funcionality.
	/// </summary>
	public class BaseViewModel : INotifyPropertyChanged
	{
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
		protected void OnRecordCreated<T>()
		{
			OnDisplayMessage(
				new DisplayMessageEventArgs
				{
					Message = $"{typeof(T).Name} created successfully.",
					Тype = DisplayMessageType.Info
				});
		}

		#endregion

		/// <summary>
		/// Represents a state if the view model is ready to exit. 
		/// For example if there is unsaved changes, should be false. Default value is true.
		/// </summary>
		/// <returns>Value indicating if the view model is ready to exit.</returns>
		public virtual bool CanExit()
		{
			return true;
		}

		/// <summary>
		/// Sets all the fields to their default values in the view model.
		/// </summary>
		public virtual void Clear()
		{
		}
	}
}
