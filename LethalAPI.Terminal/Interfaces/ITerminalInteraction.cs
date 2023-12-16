using LethalAPI.LibTerminal.Models;

namespace LethalAPI.LibTerminal.Interfaces
{
	/// <summary>
	/// The base interface for all terminal interactions
	/// </summary>
	/// <remarks>
	/// <para>
	/// You can find general implementations of this under the interactions namespace (see <seealso cref="Interactions.ConfirmInteraction"/> and <seealso cref="Interactions.TerminalInteraction"/>)
	/// </para>
	/// <para>
	/// You can also implement this yourself to implement custom interaction patterns and handlers
	/// </para>
	/// </remarks>
	public interface ITerminalInteraction
	{
		/// <summary>
		/// The prompt displayed to the user when this interaction is returned from a command
		/// </summary>
		/// <remarks>
		/// This acts as the command response, and prompt for further information
		/// </remarks>
		TerminalNode Prompt { get; }

		/// <summary>
		/// The service collection that command context will be registered to
		/// </summary>
		/// <remarks>
		/// You can use this to inject services into your own handler delegates, or fetch command context such as the <seealso cref="Terminal"/> instance when <seealso cref="HandleTerminalResponse(ArgumentStream)"/> is being run
		/// </remarks>
		ServiceCollection Services { get; }

		/// <summary>
		/// The handler for the terminal interaction. This method receives the next line of input from the terminal
		/// </summary>
		/// <remarks>
		/// You can return <see langword="null"/> to yield execution to either a parent interaction, or the command handler
		/// </remarks>
		/// <param name="arguments">Arguments provided by the user. This is the full argument input, including the first word that is commonly taken as the command name</param>
		/// <returns>A <seealso cref="TerminalNode"/>, another <seealso cref="ITerminalInteraction"/>, an object that represents the response text from this interaction, or <see langword="null"/></returns>
		object HandleTerminalResponse(ArgumentStream arguments);
	}
}
