using System;
using System.Reflection;
using LethalAPI.LibTerminal.Models;

namespace LethalAPI.LibTerminal.Interfaces
{
    /// <summary>
    /// Defines a command invoker constructor, which is responsible for transforming user input into an invoker delegate to execute a command
    /// </summary>
    public interface ICommandInvoker
    {
        /// <summary>
        /// Attempts to create an invoker for the specified command method
        /// </summary>
        /// <param name="arguments">User provided command arguments</param>
        /// <param name="services">Services available to be injected into the method</param>
        /// <param name="method">The target method to execute</param>
        /// <param name="invoker">The resulting invoker delegate</param>
        /// <returns><see langword="true"/> if an invoker could be created using the provided arguments, otherwise <see langword="false"/></returns>
        bool TryCreateInvoker(ArgumentStream arguments, ServiceCollection services, MethodInfo method, out Func<object, object?>? invoker);
    }
}
