namespace RecipeOrganiser.Utils.Events
{
	/// <summary>
	/// Type of the DisplayMessage event.
	/// </summary>
	public enum DisplayMessageType
	{
		None,
		Error,
		Info
	}

	/// <summary>
	/// Event arguments for  DisplayMessage event.
	/// </summary>
	public class DisplayMessageEventArgs
	{
		public string Message { get; set; }
		public DisplayMessageType Тype { get; set; }
	}
}
