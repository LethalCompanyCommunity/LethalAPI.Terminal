using System;
using CustomTerminalCommands.Models;

namespace CustomTerminalCommands.Attributes
{
	/// <summary>
	/// Specifies what permission level is required for this command
	/// </summary>
	/// <remarks>
	/// This attribute can be inherited, to allow you to implement your own access control decorators. See <seealso cref="AllowedCallerAttribute.CheckAllowed"/>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class AllowedCallerAttribute : Attribute
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
		public virtual bool CheckAllowed()
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
