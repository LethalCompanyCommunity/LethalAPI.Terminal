using System;
using LethalAPI.TerminalCommands.Models;

namespace LethalAPI.TerminalCommands.Attributes
{
	/// <summary>
	/// Specifies what permission level is required for this command
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class AllowedCallerAttribute : AccessControlAttribute
	{
		/// <summary>
		/// The required permission level
		/// </summary>
		public AllowedCaller Caller { get; }

		/// <summary>
		/// Sets the required permission level for the command
		/// </summary>
		/// <param name="caller">Permission level</param>
		public AllowedCallerAttribute(AllowedCaller caller)
		{
			Caller = caller;
		}

		/// <summary>
		/// Checks if the local player has permission to execute the command
		/// </summary>
		/// <returns><see langword="true"/> if the player has permission to execute the command </returns>
		public override bool CheckAllowed()
		{
			switch (Caller)
			{
				case AllowedCaller.None:
					return false;

				case AllowedCaller.Player:
					return true;

				case AllowedCaller.Host:
					if (StartOfRound.Instance == null)
					{
						return false;
					}

					return StartOfRound.Instance.IsHost;
				default:
					return true;
			}
		}
	}
}
