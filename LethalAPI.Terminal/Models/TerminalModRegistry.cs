using System;
using System.Collections.Generic;

namespace LethalAPI.LibTerminal.Models
{
    /// <summary>
    /// A local terminal command registry for a mod. Allows all commands registered to an instance to be deregistered
    /// </summary>
    public class TerminalModRegistry
    {
        /// <summary>
        /// Command instances registered to this instance
        /// </summary>
        public List<TerminalCommand> Commands { get; } = new List<TerminalCommand>();

        /// <summary>
        /// String converters registered to this instance
        /// </summary>
        public List<RegisteredStringConverter> StringConverters { get; } = new List<RegisteredStringConverter>();

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

            StringConverters.AddRange(StringConverter.RegisterFrom(instance));

            return instance;
        }

        /// <summary>
        /// Registers commands from the specified class instance
        /// </summary>
        /// <typeparam name="T">Generic class type</typeparam>
        /// <param name="instance">Instance to execute commands in</param>
        public object RegisterFrom(Type type, object instance)
        {
            foreach (var method in TerminalRegistry.GetCommandMethods(type))
            {
                var commandInstance = TerminalCommand.FromMethod(method, instance);

                TerminalRegistry.RegisterCommand(commandInstance);

                lock (Commands)
                {
                    Commands.Add(commandInstance);
                }
            }

            StringConverters.AddRange(StringConverter.RegisterFrom(type, instance));

            return instance;
        }

        /// <summary>
        /// De-registers all commands and string converters that were previously registered to this container.
        /// </summary>
        public void Deregister()
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                TerminalRegistry.Deregister(Commands[i]);
            }
            Commands.Clear();

            for (int i = 0; i < StringConverters.Count; i++)
            {
                StringConverter.Deregister(StringConverters[i]);
            }
            StringConverters.Clear();
        }
    }
}
