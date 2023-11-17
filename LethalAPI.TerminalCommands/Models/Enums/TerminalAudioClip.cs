namespace LethalAPI.TerminalCommands.Models.Enums
{
	/// <summary>
	/// Provides a list of audio effects that can be played from a <seealso cref="TerminalNode"/>
	/// </summary>

	public enum TerminalAudioClip : int
	{
		///// <summary>
		///// The entered terminal sound effect
		///// </summary>
		///// <remarks>
		///// Client-side only
		///// </remarks>
		//EnterTerminal = -3,

		///// <summary>
		///// The leave terminal sound effect
		///// </summary>
		///// <remarks>
		///// Client-side only
		///// </remarks>
		//LeaveTerminal = -2,

		///// <summary>
		///// The code broadcasted sound effect
		///// </summary>
		///// <remarks>
		///// Client-side only
		///// </remarks>
		//CodeBroadcast = -1,

		/// <summary>
		/// Plays a random item purchased sound effect
		/// </summary>
		/// <remarks>
		/// Network synced audio effect
		/// </remarks>
		ItemPurchased = 0,

		/// <summary>
		/// The Error / Unknown Command sound effect
		/// </summary>
		/// <remarks>
		/// Network synced audio effect
		/// </remarks>
		Error = 1,

		/// <summary>
		/// The original purpose of this clip is currently unknown
		/// </summary>
		/// <remarks>
		/// Network synced audio effect
		/// </remarks>
		UnknownEffect = 2
	}
}
