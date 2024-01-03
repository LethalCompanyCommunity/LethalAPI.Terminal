using System;

namespace LethalAPI.LibTerminal.Attributes
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
        /// Specifies if the command should be displayed in terminal help menus
        /// </summary>
        public bool ShowHelp { get; }

        /// <summary>
        /// Used to decorate methods to auto-register terminal commands
        /// </summary>
        /// <param name="name">The case-insensitive command name</param>
        /// <param name="clearText">When <see langword="true"/>, the text in the terminal will be cleared before writing the response</param>
        /// <param name="showHelp">When <see langword="true"/>, the help text will not be shown in the terminal</param>
        /// <remarks>
        /// <paramref name="clearText"/> is only valid for commands that do not return <seealso cref="TerminalNode"/>.
        /// </remarks>
        public TerminalCommandAttribute(string name, bool clearText = false, bool showHelp = true)
		{
			CommandName = name;
			ClearText = clearText;
			ShowHelp = showHelp;
		}
	}
}
