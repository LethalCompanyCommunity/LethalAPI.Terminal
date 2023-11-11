using System;

namespace LCAPI.TerminalCommands.Attributes
{
	/// <summary>
	/// The base attribute type for decorating terminal command methods with access restrictions.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public abstract class AccessControlAttribute : Attribute
	{
		/// <summary>
		/// Checks if the local player has the required permission level
		/// </summary>
		/// <returns><see langword="true"/> means the player does have permission, <see langword="false"/> otherwise</returns>
		public abstract bool CheckAllowed();
	}
}
