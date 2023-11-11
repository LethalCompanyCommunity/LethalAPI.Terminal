using System.Collections.Generic;

namespace LCAPI.TerminalCommands.Models
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

		/// <summary>
		/// Creates a new instance of the specified type, and registers all commands from it
		/// </summary>
		/// <typeparam name="T">The type to register commands from</typeparam>
		public void RegisterFrom<T>() where T : class, new()
		{
			RegisterFrom(new T());
		}

		/// <summary>
		/// Registers commands from the specified class instance
		/// </summary>
		/// <typeparam name="T">Generic class type</typeparam>
		/// <param name="instance">Instance to execute commands in</param>
		public void RegisterFrom<T>(T instance) where T : class
		{
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
		/// De-registers all commands that were previously registered to this container.
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
