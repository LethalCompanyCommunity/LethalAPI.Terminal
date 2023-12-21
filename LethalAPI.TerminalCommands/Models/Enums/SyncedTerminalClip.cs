namespace LethalAPI.TerminalCommands.Models.Enums
{
	/// <summary>
	/// Provides a list of audio effects that can be played from a <seealso cref="TerminalNode"/>
	/// </summary>

	public enum SyncedTerminalClip : int
	{
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
		/// Plays the telegraph-like broadcast effect played when loading some log entries
		/// </summary>
		/// <remarks>
		/// Network synced audio effect
		/// </remarks>
		BroadcastEffect = 2
	}
}