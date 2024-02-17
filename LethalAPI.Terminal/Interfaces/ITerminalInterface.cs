using LethalAPI.LibTerminal.Models;

namespace LethalAPI.LibTerminal.Interfaces
{
	public interface ITerminalInterface
	{
		/// <summary>
		/// Enables or disables the command result text post-processing done by the Terminal API. A value of <see langword="false"/> disables this type of text post-processing
		/// </summary>
		/// <remarks>
		/// <para>
		/// This post processing includes appending newlines to the end of commands, to prevent user input from continuing from the final line of the response. This behaviour might be undesirable for some custom interfaces.
		/// </para>
		/// <para>
		/// Generally, you should set this to <see langword="true"/>, unless you want to implement some form of command prompt to the left of the user input field, like in cmd or bash.
		/// </para>
		/// <para>
		/// However, if you do disable this, you will want to perform your own processing to ensure command results end with a newline, or it might cause the terminal caret/typing position to be unpredictable.
		/// </para>
		/// </remarks>
		bool APITextPostProcessing { get; }

		/// <summary>
		/// Enables or disables the command result text post-processing done by Lethal Company. A value of <see langword="false"/> will disable this type of text post-processing.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This post processing includes replacing keywords such as '[currentScannedEnemiesList]', and '[scanForItems]' with dynamically generated text parts.
		/// </para>
		/// <para>
		/// This functionality might be useful for re-implementing or creating variants of vanilla commands, but might in certain situations might cause undesirable changes to your command responses.
		/// </para>
		/// <para>
		/// <para>---</para>
		/// Generally, you should set this to <see langword="true"/>. If you want to find out more about what this does, look at decompiled source for `<seealso cref="Terminal.TextPostProcess(string, TerminalNode)"/>`
		/// </para>
		/// </remarks>
		bool VanillaTextPostProcessing { get; }

		/// <summary>
		/// Handles user input to the terminal
		/// </summary>
		/// <param name="instance">The terminal instance that raised the message</param>
		/// <param name="arguments">The full arguments provided by the player</param>
		/// <returns>A terminal node representing the response, or <see langword="null"/> if execution should cascade to the game</returns>
		/// <remarks>
		/// <para>
		///  Returning <see langword="null"/> will not yield execution to custom commands, so if you want to support command execution from other mods, you will need to implement support for it yourself.
		/// </para>
		/// <para>
		/// You can use <seealso cref="CommandHandler.ExecuteInteractions(ArgumentStream, Terminal)"/> and <seealso cref="CommandHandler.ExecuteCommand(string, ArgumentStream, Terminal)"/> to try to execute user input as a command
		/// </para>
		/// <para>
		/// If These methods both return null, the user input was not handled in an interaction or command.
		/// </para>
		/// </remarks>
		TerminalNode HandleInput(Terminal instance, ArgumentStream arguments, string rawInput);

		/// <summary>
		/// <para>
		/// Performs custom text pre-processing before text is written to the terminal.
		/// </para>
		/// <para>
		/// This modifies text after the Terminal API modifies text, and before Vanilla text post processing.
		/// </para>
		/// </summary>
		/// <param name="instance">The terminal instance that the text is being written to</param>
		/// <param name="text">The text that is going to be written to the screen</param>
		/// <returns>The modified text to write to the screen</returns>
		/// <remarks>
		/// If you do not want to do any text pre-processing, just return <paramref name="text"/>.
		/// </remarks>
		string PreProcessText(Terminal instance, string text);

		/// <summary>
		/// <para>
		/// Performs custom text post-processing, before text is written to the terminal.
		/// </para>
		/// <para>
		/// This modifies text after the Terminal API, and vanilla text post processing. Text returned from this is directly written to the terminal.
		/// </para>
		/// </summary>
		/// <param name="terminal">The terminal instance the text will be written to</param>
		/// <param name="text">The text that has had Vanilla text post processing applied to, given <seealso cref="VanillaTextPostProcessing"/> is false.</param>
		/// <returns>The final text that will be written to the terminal</returns>
		/// <remarks>
		/// If you do not want to do any text post-processing, just return <paramref name="text"/>
		/// </remarks>
		string PostProcessText(Terminal terminal, string text);

		/// <summary>
		/// Loads the default response / 'Splash Screen' that is seen when the player enters the terminal
		/// </summary>
		/// <param name="terminal">The terminal instance that the local user just entered</param>
		/// <returns>The default 'splash screen' terminal node</returns>
		/// <remarks>
		/// This node is loaded after the vanilla node, so it is recommended to set <seealso cref="TerminalNode.clearPreviousText"/> to <see langword="true"/>
		/// </remarks>
		TerminalNode GetSplashScreen(Terminal terminal);
	}
}
