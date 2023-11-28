using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LethalAPI.TerminalCommands.Interfaces;

namespace LethalAPI.TerminalCommands.Models
{
	/// <summary>
	/// Handles terminal command execution
	/// </summary>
	public static class CommandHandler
	{
		/// <summary>
		/// The interaction stack, for handling interaction layers
		/// </summary>
		private static readonly Stack<ITerminalInteraction> Interactions = new Stack<ITerminalInteraction>();

		/// <summary>
		/// Regex to split a command by spaces, while grouping sections of a command in quotations (")
		/// </summary>
		private static readonly Regex m_SplitRegex = new Regex(@"[\""](.+?)[\""]|([^ ]+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

		/// <summary>
		/// Orders <seealso cref="TerminalCommand"/> instances by descending by priority, then by argument count
		/// </summary>
		private static readonly CommandComparer m_Comparer = new CommandComparer();

		/// <summary>
		/// Finds a list of matching command candidates, then tries to execute them in weighted order, returning the first response provided.
		/// </summary>
		/// <param name="command">Command text to parse and execute</param>
		/// <param name="terminal">Terminal instance that raised the command</param>
		/// <returns>A <seealso cref="TerminalNode"/> response, or <see langword="null"/> if execution should fall-through to the game's command handler</returns>
		public static TerminalNode TryExecute(string command, Terminal terminal)
		{
			var matches = m_SplitRegex.Matches(command.Trim());
			var commandParts = matches.Cast<Match>().Select(x => x.Value.Trim('"', ' '));

			if (Interactions.TryPop(out var interaction))
			{
				var interactionStream = new ArgumentStream(commandParts.ToArray());

				var interactionResult = interaction.HandleTerminalResponse(terminal, interactionStream);

				if (interactionResult != null)
				{
					return HandleCommandResult(interactionResult);
				}
			}

			var commandName = commandParts.First();
			var commandArguments = commandParts.Skip(1).ToArray();

			var candidateCommands = new List<(TerminalCommand command, Func<TerminalNode> invoker)>();

			var overloads = TerminalRegistry.GetCommands(commandName).ToArray();

			for (int i = 0; i < overloads.Length; i++)
			{
				var registeredCommand = overloads[i];

				if (!registeredCommand.CheckAllowed())
				{
					continue;
				}

				if (!registeredCommand.TryCreateInvoker(commandArguments, terminal, out var invoker))
				{
					continue;
				}

				// A pass-though delegate to execute interactions, and return the response `TerminalNode` or null
				var passThrough = () => HandleCommandResult(invoker());

				candidateCommands.Add((registeredCommand, passThrough));
			}

			var ordered = candidateCommands.OrderByDescending(x => x.command, m_Comparer);

			foreach (var (registeredCommand, invoker) in ordered)
			{
				var result = invoker();

				if (result != null)
				{
					return result;
				}
			}
			return null;
		}

		private static TerminalNode HandleCommandResult(object result)
		{
			if (result is TerminalNode node)
			{
				return node;
			}

			if (result is ITerminalInteraction interaction)
			{
				SetInteraction(interaction);
				return interaction.Prompt;
			}

			return null;
		}

		/// <summary>
		/// Pushes an interaction onto the terminal interaction stack, to execute next
		/// </summary>
		/// <param name="interaction">Interaction to run</param>
		public static void SetInteraction(ITerminalInteraction interaction)
		{
			Interactions.Push(interaction);
		}
	}
}
