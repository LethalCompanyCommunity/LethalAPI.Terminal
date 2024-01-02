using System;

namespace LethalAPI.LibTerminal.Attributes
{
    /// <summary>
    /// Marks a method as a string converter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// String converter methods must take a single argument of a <see langword="string"/>, and return the object type they convert the string to.
    /// </para>
    /// <para>
    /// String converters may throw <seealso cref="ArgumentException"/> if the string cannot be parsed.
    /// </para>
    /// <para>
    /// String converters must be registered using <seealso cref="TerminalRegistry.RegisterFrom{T}(T)"/>
    /// </para>
    /// </remarks>
    public sealed class StringConverterAttribute : Attribute
	{
	}
}