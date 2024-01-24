using System;
using System.Reflection;
using LethalAPI.LibTerminal.Interfaces;

namespace LethalAPI.LibTerminal.Models
{
    /// <summary>
    /// The default command invoker, utalizing the common implementation in <seealso cref="CommandActivator"/>
    /// </summary>
    public class DefaultCommandInvoker : ICommandInvoker
    {
        /// <inheritdoc/>
        public bool TryCreateInvoker(ArgumentStream arguments, ServiceCollection services, MethodInfo method, out Func<object, object?>? invoker)
        {
            return CommandActivator.TryCreateInvoker(arguments, services, method, out invoker);
        }
    }
}
