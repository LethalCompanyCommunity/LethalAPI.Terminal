using LethalAPI.TerminalCommands.Models;

namespace LethalAPI.TerminalCommands.Interfaces
{
	public interface ITerminalInteraction
	{
		TerminalNode Prompt { get; }

		object HandleTerminalResponse(Terminal terminal, ArgumentStream arguments);
	}
}
