using System.Collections.Generic;
using BepInEx.Logging;

namespace CustomTerminalCommands.Models
{
	/// <summary>
	/// A local terminal command registry for a mod. Allows all commands registered to an instance to be deregistered
	/// </summary>
	public class ModCommands
	{
		/// <summary>
		/// Command instances registered to this instance
		/// </summary>
		public List<TerminalCommand> Commands { get; } = new List<TerminalCommand>();


		private readonly ManualLogSource _logSource = new ManualLogSource("LCAPI.TerminalCommands");

		/// <summary>
		/// Creates a new instance of the speci specified type, and registers all commands from it
		/// </summary>
		/// <typeparam name="T">The type to register commands from</typeparam>
		public void RegisterFrom<T>() where T : class, new()
		{
			_logSource.LogInfo($"Registering from type");
			RegisterFrom(new T());
		}

		/// <summary>
		/// Registers commands from the specified class instance
		/// </summary>
		/// <typeparam name="T">Generic class type</typeparam>
		/// <param name="instance">Instance to execute commands in</param>
		public void RegisterFrom<T>(T instance) where T : class
		{
			_logSource.LogInfo("Registering from type instance");
			foreach (var method in CommandRegistry.GetCommandMethods<T>())
			{
				var commandInstance = TerminalCommand.FromMethod(method, instance);

				CommandRegistry.RegisterCommand(commandInstance);

				lock (Commands)
				{
					Commands.Add(commandInstance);
				}
			}
		}

		/// <summary>
		/// Deregisters all commands that were previously registered to this container.
		/// </summary>
		public void Deregister()
		{
			if (Commands == null)
			{
				return;
			}

			for (int i = 0; i < Commands.Count; i++)
			{
				CommandRegistry.Deregister(Commands[i]);
			}
		}
	}
}
