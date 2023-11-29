using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LethalAPI.TerminalCommands.Interfaces;
using UnityEngine;

namespace LethalAPI.TerminalCommands.Models
{
	/// <summary>
	/// Handles terminal command execution
	/// </summary>
	public static class CommandHandler
	{
		/// <summary>
		/// The current interaction depth.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is the current number of interactions active, all of which need to be executed before terminal input will fall through to command execution.
		/// </para>
		/// <para>
		/// A value of 0 indicates no terminal interaction is active.
		/// </para>
		/// </remarks>
		public static int InteractionDepth => m_Interactions.Count;

		/// <summary>
		/// The current terminal interface all terminal input is being redirected to, or <see langword="null"/> if no interface is overriding the terminal system
		/// </summary>
		public static ITerminalInterface CurrentInterface { get; private set; }

		/// <summary>
		/// The interaction stack, for handling interaction layers
		/// </summary>
		private static readonly Stack<ITerminalInteraction> m_Interactions = new Stack<ITerminalInteraction>();

		/// <summary>
		/// Regex to split a command by spaces, while grouping sections of a command in quotations (")
		/// </summary>
		private static readonly Regex m_SplitRegex = new Regex(@"[\""](.+?)[\""]|([^ ]+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

		/// <summary>
		/// Orders <seealso cref="TerminalCommand"/> instances by descending by priority, then by argument count
		/// </summary>
		private static readonly CommandComparer m_Comparer = new CommandComparer();

		/// <summary>
		/// Handles user input from the terminal. In order, this executes Terminal interfaces, Interactions, then custom commands, null cascading through them.
		/// </summary>
		/// <param name="command">The terminal command input</param>
		/// <param name="terminal">Terminal instance that raised the command</param>
		/// <returns>A <seealso cref="TerminalNode"/> response, or <see langword="null"/> if execution should fall-through to the game's command handler</returns>
		public static TerminalNode HandleCommandInput(string command, Terminal terminal)
		{
			// Split terminal input into parts
			var matches = m_SplitRegex.Matches(command.Trim());
			var commandParts = matches.Cast<Match>().Select(x => x.Value.Trim('"', ' ')).ToArray();

			var interactionStream = new ArgumentStream(commandParts);


			if (CurrentInterface != null)
			{
				// Redirect all terminal input to the current interface
				return CurrentInterface.HandleInput(terminal, interactionStream);
			}

			// Handle interactions if any
			var interactionResult = ExecuteInteractions(interactionStream, terminal);

			if (interactionResult != null)
			{
				return interactionResult;
			}

			// Handle command interpretation

			var commandName = commandParts.First();
			var commandArguments = new ArgumentStream(commandParts.Skip(1));

			return ExecuteCommand(commandName, commandArguments, terminal);
		}

		/// <summary>
		/// Executes active interactions, if any.
		/// </summary>
		/// <param name="arguments">Full user input arguments, including the first word/'command name'</param>
		/// <param name="terminal">The terminal instance that raised the input</param>
		/// <returns>A <seealso cref="TerminalNode"/> representing a response from an interaction, or null if the input should be parsed as a command</returns>
		public static TerminalNode ExecuteInteractions(ArgumentStream arguments, Terminal terminal)
		{
			while (m_Interactions.TryPop(out var interaction))
			{
				arguments.Reset();
				try
				{
					// Fetch the service collection provided by the interaction, and add default services to it
					var interactServices = interaction.Services;
					interactServices.WithServices(arguments, terminal, arguments.Arguments);

					// Handle execution of the interaction
					var interactionResult = interaction.HandleTerminalResponse(arguments);

					// Return result, or null cascade
					if (interactionResult != null)
					{
						return HandleCommandResult(interactionResult);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.GetType().Name);
					Console.WriteLine($"Error executing interaction: {ex.Message}, {ex.StackTrace}");
				}
			}

			return null;
		}

		/// <summary>
		/// Executes a custom command from user input. 
		/// </summary>
		/// <param name="commandName">The case-insensitive name of the command to execute</param>
		/// <param name="arguments">The arguments to supply to the command</param>
		/// <param name="terminal">The terminal instance that raised the input</param>
		/// <returns>A resulting <seealso cref="TerminalNode"/> that represents the immediate response of the command.</returns>
		/// <remarks>
		/// Commands may also enter a terminal interaction, which consumes the next terminal input. 
		/// Interactions can be handled by calling <seealso cref="ExecuteInteractions(ArgumentStream, Terminal)"/>, and only executing this method if it returns null
		/// </remarks>
		public static TerminalNode ExecuteCommand(string commandName, ArgumentStream arguments, Terminal terminal)
		{
			// Handle command interpretation

			var candidateCommands = new List<(TerminalCommand command, Func<TerminalNode> invoker)>();

			var overloads = TerminalRegistry.GetCommands(commandName).ToArray();

			var services = new ServiceCollection(arguments, arguments.Arguments, terminal);

			// Evaluate candidates

			for (int i = 0; i < overloads.Length; i++)
			{
				var registeredCommand = overloads[i];

				if (!registeredCommand.CheckAllowed())
				{
					continue;
				}

				arguments.Reset();
				if (!registeredCommand.TryCreateInvoker(arguments, services, out var invoker))
				{
					continue;
				}

				// A pass-though delegate to execute interactions, and return the response `TerminalNode` or null
				var passThrough = () => HandleCommandResult(invoker());

				candidateCommands.Add((registeredCommand, passThrough));
			}

			// Execute candidates
			var ordered = candidateCommands.OrderByDescending(x => x.command, m_Comparer); // Order candidates descending by priority, then argument count

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

		/// <summary>
		/// Handles the responses from commands, executing actions as needed
		/// </summary>
		/// <param name="result">Result to parse into a <seealso cref="TerminalNode"/></param>
		/// <returns><seealso cref="TerminalNode"/> command display response</returns>
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

			return ScriptableObject.CreateInstance<TerminalNode>()
											.WithDisplayText(result);
		}

		/// <summary>
		/// Pushes an interaction onto the terminal interaction stack, to execute next
		/// </summary>
		/// <param name="interaction">Interaction to run</param>
		public static void SetInteraction(ITerminalInteraction interaction)
		{
			m_Interactions.Push(interaction);
		}

		/// <summary>
		/// Sets the current terminal interface, to redirect all terminal input to.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This can be used to create your own fully custom terminal systems and handlers, giving you full control over the terminal
		/// </para>
		/// <para>
		/// If your interface is meant to be persistent, is recommended you implement some way to handle commands, otherwise commands from all other mods will become unavailable, potentially breaking mods and compatibility.
		/// </para>
		/// <para>
		/// You can implement command support by calling <seealso cref="ExecuteInteractions(ArgumentStream, Terminal)"/> and if it returns <see langword="null"/>, call <seealso cref="ExecuteCommand(string, ArgumentStream, Terminal)"/>.
		/// </para>
		/// </remarks>
		/// <param name="terminalInterface">The terminal interface to redirect all terminal input to</param>
		public static void SetInterface(ITerminalInterface terminalInterface)
		{
			CurrentInterface = terminalInterface;
		}

		/// <summary>
		/// Resets the terminal interface, removing any current <seealso cref="ITerminalInterface"/>.
		/// </summary>
		/// <remarks>
		/// Use of this method is discouraged under most circumstances, for most cases, use <seealso cref="ResetInterface{T}()"/> instead.
		/// </remarks>
		public static void ResetInterface()
		{
			CurrentInterface = null;
		}

		/// <summary>
		/// Conditionally resets the terminal interface, so long as the current interface is of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The terminal interface type to clear</typeparam>
		/// <remarks>
		/// <para>
		/// This is a variant of <seealso cref="ResetInterface()"/>, which only clears the terminal interface if it is of the specified type.
		/// </para>
		/// <para>
		/// This method is preferred, as calling <seealso cref="ResetInterface()"/> from within a command could break third-party interfaces that support command execution (see <seealso cref="ExecuteCommand(string, ArgumentStream, Terminal)"/>)
		/// </para>
		/// </remarks>
		public static void ResetInterface<T>() where T : ITerminalInterface
		{
			if (CurrentInterface != null && CurrentInterface is T)
			{
				ResetInterface();
			}
		}
	}
}
