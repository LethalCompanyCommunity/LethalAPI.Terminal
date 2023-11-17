using System.Collections.Generic;
using System.Linq;
using System.Text;
using LethalAPI.TerminalCommands.Attributes;
using LethalAPI.TerminalCommands.Models;
using LethalAPI.TerminalCommands.Models.Configuration;

namespace LethalAPI.TerminalCommands.Commands
{
	public class ConfigurationCommands
	{
		[TerminalCommand("Config", clearText: true)]
		[CommandInfo("Shows and manages config options")]
		public StringBuilder ConfigCommand()
		{
			var message = new StringBuilder();

			message.AppendLine("Use >Config [Option] to view more options, and manage settings");
			message.AppendLine();
			message.AppendLine("OPTIONS:");

			foreach (var rootNode in TerminalRegistry.ConfigTree.Root.Children)
			{
				foreach (var groups in rootNode.Children)
				{
					message.AppendLine($">{groups.Name}");
					message.AppendLine(groups.Description);
					message.AppendLine();
				}

				foreach (var option in rootNode.Options)
				{
					message.AppendLine(option.Name);
					message.AppendLine(option.Description);
					message.AppendLine();
				}
			}

			return message;
		}

		[TerminalCommand("Config", clearText: false)]
		public StringBuilder ConfigCommand(ArgumentStream arguments)
		{
			var node = TerminalRegistry.ConfigTree.Root;
			var path = new List<string>();

			while (true)
			{
				if (!arguments.TryReadNext(out string option))
				{
					// Display help for current node
					return HandleShowGroup(node, path);
				}

				if (node.TryGetChild(option, out node))
				{
					// Iterate down
					path.Add(option);
					continue;
				}

				if (node.TryGetValue(option, out var configItem))
				{
					// Handle item
					return HandleConfigOption(configItem, path, arguments);
				}

				// Unknown group/option

				break;
			}

			var sb = new StringBuilder();

			sb.AppendLine("ERROR: Unknown config group/option.");

			return sb;
		}

		private StringBuilder HandleShowGroup(ConfigNode node, List<string> path)
		{
			var sb = new StringBuilder();
			sb.AppendLine($">{string.Join(">", path.Select(x => x.ToUpper()))}");
			sb.AppendLine($"GROUP: >{node.Name.ToUpper()}");

			foreach (var group in node.Children)
			{
				sb.AppendLine($">{group.Name.ToUpper()}");
				sb.AppendLine(group.Description.ToUpper());
				sb.AppendLine();
			}

			foreach (var option in node.Options)
			{
				sb.AppendLine(option.Name.ToUpper());
				sb.AppendLine(option.Description.ToUpper());
			}

			return sb;
		}

		private StringBuilder HandleConfigOption(ConfigItem item, List<string> path, ArgumentStream arguments)
		{
			var sb = new StringBuilder();

			if (arguments.TryReadNext(out string newValue))
			{
				if (item.TrySetValue(newValue))
				{
					sb.AppendLine("Value Updated");
					return sb;
				}

				sb.AppendLine("Failed set option: Invalid argument");

				// TODO: Create a mapping for type -> friendly description
				sb.AppendLine($"Expected type: {item.Type.Name.ToUpper()}");

				return sb;
			}

			sb.AppendLine($">{string.Join(">", path.Select(x => x.ToUpper()))}");
			sb.AppendLine($">OPTION: {item.Name.ToUpper()}");
			sb.AppendLine($">TYPE: {item.Type.Name.ToUpper()}");
			sb.AppendLine(item.Description.ToUpper());
			sb.AppendLine($"CURRENT VALUE: {item.GetValue()}");
			sb.AppendLine($"Use >Config {string.Join(" ", path)} {item.Name} [New Value]");

			return sb;
		}
	}
}
