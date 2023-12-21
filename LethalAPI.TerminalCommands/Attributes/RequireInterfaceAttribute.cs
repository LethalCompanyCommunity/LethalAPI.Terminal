using System;
using LethalAPI.TerminalCommands.Interfaces;
using LethalAPI.TerminalCommands.Models;

namespace LethalAPI.TerminalCommands.Attributes
{
	/// <summary>
	/// Specifies that a command can only be ran when the specified terminal interface is active
	/// </summary>
	public class RequireInterfaceAttribute : AccessControlAttribute
	{
		/// <summary>
		/// The <seealso cref="ITerminalInterface"/> type that needs to be active for this command to be executable
		/// </summary>
		public Type InterfaceType { get; }

		/// <summary>
		/// Specifies that a command is only valid inside of the specified <seealso cref="ITerminalInterface"/>
		/// </summary>
		/// <param name="interfaceType">The <seealso cref="ITerminalInterface"/> type to limit this command to</param>
		public RequireInterfaceAttribute(Type interfaceType)
		{
			InterfaceType = interfaceType;
		}

		/// <summary>
		/// Checks if the specified terminal interface is active
		/// </summary>
		/// <returns><see langword="true"/> if the specified terminal interface is active</returns>
		public override bool CheckAllowed()
		{
			return CommandHandler.CurrentInterface != null && CommandHandler.CurrentInterface.GetType() == InterfaceType;
		}
	}

	/// <summary>
	/// Specifies that a command can only be ran when the specified terminal interface is active
	/// </summary>
	/// <typeparam name="T">The <seealso cref="ITerminalInterface"/> type that needs to be active for this command to be executable</typeparam>
	public sealed class RequireInterfaceAttribute<T> : RequireInterfaceAttribute where T : ITerminalInterface
	{
		/// <summary>
		/// Specifies that a command is only valid inside of the specified <seealso cref="ITerminalInterface"/>
		/// </summary>
		public RequireInterfaceAttribute() : base(typeof(T))
		{
		}
	}
}
