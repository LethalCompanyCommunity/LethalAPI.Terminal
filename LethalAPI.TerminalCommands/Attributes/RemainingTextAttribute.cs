using System;

namespace LethalAPI.TerminalCommands.Attributes
{
	/// <summary>
	/// Designates that a string argument should contain the remaining text in the command
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class RemainingTextAttribute : Attribute
	{
	}
}
