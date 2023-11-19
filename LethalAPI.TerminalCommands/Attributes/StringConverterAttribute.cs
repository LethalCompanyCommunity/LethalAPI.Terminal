using System;

namespace LethalAPI.TerminalCommands.Attributes
{
	/// <summary>
	/// Used to mark a method as a string converter.
	/// </summary>
	/// <remarks>
	/// String converters should take only a single argument of a string, and return the type they convert strings to
	/// <para>
	/// String Converter methods can throw <seealso cref="ArgumentException"/> if the input string is in the incorrect format
	/// </para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class StringConverterAttribute : Attribute
	{
	}
}