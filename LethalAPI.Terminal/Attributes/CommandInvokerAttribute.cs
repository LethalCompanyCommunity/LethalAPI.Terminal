using System;
using LethalAPI.LibTerminal.Interfaces;

namespace LethalAPI.LibTerminal.Attributes
{
    /// <summary>
    /// The base attribute for specifying a custom command invoker
    /// </summary>
    /// <typeparam name="T">The command invoker type to use</typeparam>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public abstract class CommandInvokerAttribute : Attribute
    {
        /// <summary>
        /// Creates an instance of the custom command invoker
        /// </summary>
        /// <returns>Command invoker instance</returns>
        public abstract ICommandInvoker CreateInvoker();
    }

    /// <summary>
    /// Specifies the custom command invoker to use for a command
    /// </summary>
    /// <typeparam name="T">The command invoker type to use</typeparam>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CommandInvokerAttribute<T> : CommandInvokerAttribute where T : ICommandInvoker, new()
    {
        /// <inheritdoc/>
        public override ICommandInvoker CreateInvoker()
        {
            return new T();
        }
    }
}
