using System;

namespace LCAPI.TerminalCommands.Attributes
{
	/// <summary>
	/// Specifies the priority of a command.
	/// </summary>
	/// <remarks>
	/// You should not use this attribute unless you are intending to overwrite another custom terminal command
	/// </remarks>
	public sealed class CommandPriority : Attribute
	{
		/// <summary>
		/// Priority value, higher the value, the greater the priority.
		/// </summary>
		/// <remarks>
		/// A priority of 0 is default.
		/// </remarks>
		public int Priority { get; }

		/// <summary>
		/// Sets the command priority. Do not use this unless you are intending to override another custom command
		/// </summary>
		/// <param name="priority">Priority value, higher the value, the greater the priority</param>
		public CommandPriority(int priority)
		{
			Priority = priority;
		}
	}
}
