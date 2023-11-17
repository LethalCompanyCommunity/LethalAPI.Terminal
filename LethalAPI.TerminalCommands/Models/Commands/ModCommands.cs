using System.Collections.Generic;

namespace LethalAPI.TerminalCommands.Models
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
		public T RegisterFrom<T>() where T : class, new()
		{
			return RegisterFrom(new T());
		}

		/// <summary>
		/// Registers commands from the specified class instance
		/// </summary>
		/// <typeparam name="T">Generic class type</typeparam>
		/// <param name="instance">Instance to execute commands in</param>
		public T RegisterFrom<T>(T instance) where T : class
		{
			foreach (var method in TerminalRegistry.GetCommandMethods<T>())
			{
				var commandInstance = TerminalCommand.FromMethod(method, instance);

				TerminalRegistry.RegisterCommand(commandInstance);

				lock (Commands)
				{
					Commands.Add(commandInstance);
				}
			}

			TerminalRegistry.ConfigTree.RegisterFrom(typeof(T), instance);

			return instance;
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
				TerminalRegistry.Deregister(Commands[i]);
			}
		}
	}
}
