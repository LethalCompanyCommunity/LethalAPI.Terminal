using System.Linq;
using System.Text;
using LCAPI.TerminalCommands.Attributes;
using LCAPI.TerminalCommands.Models;

namespace LCAPI.TerminalCommands.Commands
{
	/// <summary>
	/// Contains Terminal Command definitions for the built-in help commands
	/// </summary>
	public class CommandInfoCommands
	{
		[TerminalCommand("Other", clearText: true)]
		public string CommandList()
		{
			var builder = new StringBuilder();

			builder.AppendLine("Other commands:");
			builder.AppendLine();
			builder.AppendLine(">VIEW MONITOR");
			builder.AppendLine("To toggle on/off the main monitor's map cam");
			builder.AppendLine();
			builder.AppendLine(">SWITCH {RADAR}");
			builder.AppendLine("To switch the player view on the main monitor");
			builder.AppendLine();
			builder.AppendLine(">PING [Radar booster name]");
			builder.AppendLine("To switch the player view on the main monitor");
			builder.AppendLine();
			builder.AppendLine(">SCAN");
			builder.AppendLine("To scan for the number of items left on the current planet");
			builder.AppendLine();

			foreach (var command in CommandRegistry.EnumerateCommands())
			{
				if (command.Description == null)
				{
					continue;
				}

				if (!command.CheckAllowed())
				{
					continue;
				}

				builder.AppendLine($">{command.Name.ToUpper()} {command.Syntax?.ToUpper()}");
				builder.AppendLine(command.Description);
				builder.AppendLine();

			}

			return builder.ToString();
		}

		[TerminalCommand("Help", clearText: false), CommandInfo("Shows further information about a command", "[Command]")]
		public string HelpCommand(string name)
		{
			var builder = new StringBuilder();
			var commands = CommandRegistry.EnumerateCommands(name).ToArray();

			if (commands.Length == 0)
			{
				return $"Unknown command: '{name}'";
			}

			foreach (var command in commands)
			{
				builder.AppendLine($">{command.Name.ToUpper()} {command.Syntax?.ToUpper()}");
				builder.AppendLine(command.Description);
				if (!command.CheckAllowed())
				{
					builder.AppendLine("[Host Only]");
				}

				builder.AppendLine();
			}

			return builder.ToString();
		}
	}
}
