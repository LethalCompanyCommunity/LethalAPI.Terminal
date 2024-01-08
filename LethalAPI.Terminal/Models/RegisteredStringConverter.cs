using System;

namespace LethalAPI.LibTerminal.Models
{
	/// <summary>
	/// Represents a registered string converter, to allow a converter to be de-registered
	/// </summary>
	public readonly struct RegisteredStringConverter
	{
		/// <summary>
		/// The resulting type of the string converter
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// The string converter instance
		/// </summary>
		public StringConversionHandler Handler { get; }

		public RegisteredStringConverter(Type type, StringConversionHandler handler)
		{
			Type = type;
			Handler = handler;
		}
	}
}
