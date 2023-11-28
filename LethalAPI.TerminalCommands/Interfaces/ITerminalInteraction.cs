using LethalAPI.TerminalCommands.Models;

namespace LethalAPI.TerminalCommands.Interfaces
{
	public interface ITerminalInteraction
	{
		TerminalNode Prompt { get; }

		ServiceCollection Services { get; }

		object HandleTerminalResponse(ArgumentStream arguments);
	}
}
