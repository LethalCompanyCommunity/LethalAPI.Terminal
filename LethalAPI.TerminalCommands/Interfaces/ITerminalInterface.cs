using LethalAPI.TerminalCommands.Models;

namespace LethalAPI.TerminalCommands.Interfaces
{
	public interface ITerminalInterface
	{
		TerminalNode HandleInput(Terminal instance, ArgumentStream arguments);
	}
}
