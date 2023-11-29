using System;
using System.Reflection;
using BepInEx.Logging;
using LethalAPI.TerminalCommands.Attributes;

namespace LethalAPI.TerminalCommands.Models
{
	/// <summary>
	/// Command instance representing a registered terminal command
	/// </summary>
	public class TerminalCommand
	{
		/// <summary>
		/// The base name of the command
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Source method the command is declared in
		/// </summary>
		public MethodInfo Method { get; }

		/// <summary>
		/// Instance of the <seealso cref="Method"/>'s parent class
		/// </summary>
		public object Instance { get; }

		/// <summary>
		/// When true, the console is cleared before writing the command response
		/// </summary>
		public bool ClearConsole { get; }

		/// <summary>
		/// Number of arguments used by this command
		/// </summary>
		public int ArgumentCount { get; }

		/// <summary>
		/// Optional command syntax
		/// </summary>
		public string Syntax { get; }

		/// <summary>
		/// Optional command description. This value being set enrolls the command to be shown in help commands
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Command execution priority, with a default of 0.
		/// </summary>
		public int Priority { get; }

		private ManualLogSource m_LogSource = new ManualLogSource("LethalAPI.TerminalCommands");

		public TerminalCommand(string name, MethodInfo method, object instance, bool clearConsole, string syntax = null, string description = null, int priority = 0)
		{
			Name = name;
			Method = method;
			Instance = instance;
			ClearConsole = clearConsole;
			ArgumentCount = method.GetParameters().Length;
			Syntax = syntax;
			Description = description;
			Priority = priority;
		}

		/// <summary>
		/// Checks if the local player has permission to execute this command
		/// </summary>
		/// <returns><see langword="true"/> if the player can execute the command</returns>
		public bool CheckAllowed()
		{
			var accessControl = Method.GetCustomAttributes<AccessControlAttribute>();

			foreach (var attribute in accessControl)
			{
				if (!attribute.CheckAllowed())
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Parses a command from an instance, optionally overriding the command name
		/// </summary>
		/// <param name="info">Method info that declares the command</param>
		/// <param name="instance">Object instance to execute the command in</param>
		/// <param name="overrideName">Optional command name override</param>
		/// <returns>Terminal command instance</returns>
		public static TerminalCommand FromMethod(MethodInfo info, object instance, string overrideName = null)
		{
			var clear = false;
			string syntax = null;
			string description = null;
			string name = overrideName;
			int priority = 0;

			var command = info.GetCustomAttribute<TerminalCommandAttribute>();
			if (command != null)
			{
				name = name ?? command.CommandName;
				clear = command.ClearText;
			}

			var commandInfo = info.GetCustomAttribute<CommandInfoAttribute>();
			if (commandInfo != null)
			{
				syntax = commandInfo.Syntax;
				description = commandInfo.Description;
			}

			var priorityValue = info.GetCustomAttribute<CommandPriority>();
			if (priorityValue != null)
			{
				priority = priorityValue.Priority;
			}

			return new TerminalCommand(name, info, instance, clear, syntax, description, priority);
		}

		/// <summary>
		/// Attempts to parse a user-entered argument list, to create an invoker for a command
		/// </summary>
		/// <param name="arguments">Arguments to use when executing the command</param>
		/// <param name="terminal">Terminal instance that raised the command</param>
		/// <param name="invoker">Delegate that executes the command using the specified arguments</param>
		/// <returns><see langword="true"/> if the provided arguments match the signature for this command, and could be parsed correctly.</returns>
		public bool TryCreateInvoker(ArgumentStream arguments, ServiceCollection services, out Func<object> invoker)
		{
			arguments.Reset();

			if (CommandActivator.TryCreateInvoker(arguments, services, Method, out var activatedInvoker))
			{
				invoker = () => activatedInvoker(Instance);
				return true;
			}

			invoker = null;
			return false;
		}
	}
}
