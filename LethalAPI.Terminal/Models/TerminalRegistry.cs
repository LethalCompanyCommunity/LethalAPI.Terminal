using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LethalAPI.LibTerminal.Attributes;

namespace LethalAPI.LibTerminal.Models
{
	/// <summary>
	/// Manages instances of terminal commands
	/// </summary>
	public class TerminalRegistry
	{
		/// <summary>
		/// Dictionary containing all registered commands. You shouldn't be interfacing with this directly, instead use the APIs exposed by this class, or <seealso cref="TerminalModRegistry"/>.
		/// You can enumerate registered commands using <seealso cref="EnumerateCommands()"/> and <seealso cref="EnumerateCommands(string)"/>
		/// </summary>
		private static readonly ConcurrentDictionary<string, List<TerminalCommand>> m_RegisteredCommands = new ConcurrentDictionary<string, List<TerminalCommand>>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// Automatically registers all terminal commands from an instance, and returns a commands <seealso cref="TerminalModRegistry"/> token, which should be used to deregister all terminal commands when your mod unloads.
		/// </summary>
		/// <typeparam name="T">Instance type</typeparam>
		/// <param name="instance">Instance to execute commands in</param>
		/// <returns>Token that can be used to register further commands, and also deregister commands when your mod unloads</returns>
		public static TerminalModRegistry RegisterFrom<T>(T instance) where T : class
		{
			var token = new TerminalModRegistry();

			foreach (var method in GetCommandMethods<T>())
			{
				var command = TerminalCommand.FromMethod(method, instance);
				RegisterCommand(command);

				token.Commands.Add(command);
			}

			StringConverter.RegisterFrom(instance);

			return token;
		}

		/// <summary>
		/// Creates a mod-specific terminal command registry, to allow registration and deregistration of commands
		/// </summary>
		/// <returns>Mod terminal command registry</returns>
		public static TerminalModRegistry CreateTerminalRegistry()
		{
			return new TerminalModRegistry();
		}

		/// <summary>
		/// Registers a command instance. <seealso cref="RegisterFrom{T}(T)"/> is preferred. This method is primarily intended for internal use 
		/// </summary>
		/// <param name="command"></param>
		public static void RegisterCommand(TerminalCommand command)
		{
			List<TerminalCommand> commands;

			if (!m_RegisteredCommands.TryGetValue(command.Name, out commands))
			{
				commands = new List<TerminalCommand>();
				m_RegisteredCommands[command.Name] = commands;
			}

			lock (commands)
			{
				commands.Add(command);
			}
		}

		/// <summary>
		/// De-registers a command instance. You should call <seealso cref="TerminalModRegistry.Deregister"/> (returned by <seealso cref="RegisterFrom{T}(T)"/>) instead.
		/// </summary>
		/// <remarks>
		/// Primarily intended for internal use
		/// </remarks>
		/// <param name="command">Command instance to deregister</param>
		public static void Deregister(TerminalCommand command)
		{
			if (!m_RegisteredCommands.TryGetValue(command.Name, out var overloads))
			{
				return;
			}

			lock (overloads)
			{
				overloads.Remove(command);
			}
		}

		/// <summary>
		/// Fetches all commands by a command name
		/// </summary>
		/// <param name="commandName">Name of the commands to fetch</param>
		/// <returns>List of commands</returns>
		public static IReadOnlyList<TerminalCommand> GetCommands(string commandName)
		{
			if (m_RegisteredCommands.TryGetValue(commandName, out var commands))
			{
				return commands;
			}

			return new List<TerminalCommand>();
		}

		/// <summary>
		/// Enumerates registered commands/overloads for a specific command name
		/// </summary>
		/// <param name="name">Name of the command/s to enumerate</param>
		/// <returns>Command enumerable</returns>
		public static IEnumerable<TerminalCommand> EnumerateCommands(string name)
		{
			if (!m_RegisteredCommands.TryGetValue(name, out var overloads))
			{
				return Enumerable.Empty<TerminalCommand>();
			}
			return overloads;
		}

		/// <summary>
		/// Enumerates all commands registered to the container
		/// </summary>
		/// <returns>All terminal command instances</returns>
		public static IEnumerable<TerminalCommand> EnumerateCommands()
		{
			var keys = m_RegisteredCommands.Keys.ToArray();

			for (int i = 0; i < keys.Length; i++)
			{
				var overloads = m_RegisteredCommands[keys[i]];

				for (int c = 0; c < overloads.Count; c++)
				{
					yield return overloads[c];
				}
			}
		}

		/// <summary>
		/// Enumerates all methods decorated with <seealso cref="TerminalCommandAttribute"/> from a type
		/// </summary>
		/// <typeparam name="T">Type of enumerate command methods from</typeparam>
		/// <returns>Enumerable of valid terminal command methods</returns>
		public static IEnumerable<MethodInfo> GetCommandMethods<T>()
		{
			foreach (var method in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (method.GetCustomAttribute<TerminalCommandAttribute>() == null)
				{
					continue;
				}

				yield return method;
			}
		}
	}
}
