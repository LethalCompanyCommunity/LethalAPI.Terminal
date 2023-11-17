using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LethalAPI.TerminalCommands.Models
{
	/// <summary>
	/// Handles terminal command execution
	/// </summary>
	public static class CommandHandler
	{
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

			var commandName = commandParts.First();
			var commandArguments = commandParts.Skip(1).ToArray();

			var candidateCommands = new List<(TerminalCommand command, Func<TerminalNode> invoker)>();

			var overloads = TerminalRegistry.GetCommands(commandName).ToArray();

			for (int i = 0; i < overloads.Length; i++)
			{
				var registeredCommand = overloads[i];

				Console.WriteLine($"Checking candidate: {registeredCommand.Name}({registeredCommand.ArgumentCount})");

				if (!registeredCommand.CheckAllowed())
				{
					Console.WriteLine($"Checking Not Allowed");

					continue;
				}

				if (!registeredCommand.TryCreateInvoker(commandArguments, terminal, out var invoker))
				{
					Console.WriteLine($"Failed to create invoker");
					continue;
				}
				Console.WriteLine($"Valid!");

				candidateCommands.Add((registeredCommand, invoker));
			}

			Console.WriteLine($"Candidates: {candidateCommands.Count}");

			var ordered = candidateCommands.OrderByDescending(x => x.command, m_Comparer);

			foreach (var (registeredCommand, invoker) in ordered)
			{
				Console.WriteLine($"Invoking {registeredCommand.Name}({registeredCommand.ArgumentCount})");

				var result = invoker();

				if (result != null)
				{
					return result;
				}
			}
			return null;
		}
	}
}
