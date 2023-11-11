using System;

namespace CustomTerminalCommands.Attributes
{
	/// <summary>
	/// Used to decorate methods to auto-register terminal commands
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class TerminalCommandAttribute : Attribute
	{
		/// <summary>
		/// The name of the command
		/// </summary>
		public string CommandName { get; }

		/// <summary>
		/// Specifies if the terminal text should be cleared on command execution
		/// </summary>
		public bool ClearText { get; }

		/// <summary>
		/// Used to decorate methods to auto-register terminal commands
		/// </summary>
		/// <param name="name">The case-insensitive command name</param>
		/// <param name="clearText">When <see langword="true"/>, the text in the terminal will be cleared before writing the response</param>
		/// <remarks>
		/// <paramref name="clearText"/> is only valid for commands that do not return <seealso cref="TerminalNode"/>.
		/// </remarks>
		public TerminalCommandAttribute(string name, bool clearText = false)
		{
			CommandName = name;
			ClearText = clearText;
		}
	}
}
