using System;
using System.Linq;

namespace CustomTerminalCommands.Models
{
	/// <summary>
	/// Provides an easy interface for forward parsing an array of string arguments
	/// </summary>
	public class ArgumentStream
	{
		/// <summary>
		/// Underluing argument array
		/// </summary>
		public string[] Arguments { get; }

		/// <summary>
		/// Current read index in the arguments
		/// </summary>
		public int Index { get; set; } = 0;

		/// <summary>
		/// When <see langword="true"/>, there are no more arguments to read
		/// </summary>
		public bool EndOfStream => Index >= Arguments.Length;

		/// <summary>
		/// Creates a new argument stream from an array
		/// </summary>
		/// <param name="arguments">Arguments to read</param>
		public ArgumentStream(string[] arguments)
		{
			Arguments = arguments;
		}

		/// <summary>
		/// Resets the read index back to 0
		/// </summary>
		public void Reset()
		{
			Index = 0;
		}

		/// <summary>
		/// Tries to parse the next string as the specified type. Supports primitives, and some custom types defined in <seealso cref="StringConverter"/>
		/// </summary>
		/// <param name="type">The type to parse the string as</param>
		/// <param name="value">Resulting object instance</param>
		/// <returns><see langword="true"/> if the string could be parsed as the specified type</returns>
		public bool TryReadNext(Type type, out object value)
		{
			if (EndOfStream)
			{
				value = null;
				return false;
			}

			return StringConverter.TryConvert(Arguments[Index++], type, out value);
		}

		/// <summary>
		/// Reads all the remaining text in the argument stream. Does not modify the read index.
		/// </summary>
		/// <param name="result">The remaining text in the stream</param>
		/// <returns><see langword="true"/> if there was next to read, otherwise the end of the stream has been reached</returns>
		public bool TryReadRemaining(out string result)
		{
			if (EndOfStream)
			{
				result = null;
				return false;
			}

			result = string.Join(" ", Arguments.Skip(Index));
			return true;
		}

		#region "Generic Try Read"

		/// <summary>
		/// Returns the next string in the stream
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached</returns>
		public bool TryReadNext(out string value)
		{
			value = string.Empty;
			if (EndOfStream)
			{
				return false;
			}
			value = Arguments[Index++];
			return true;
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out sbyte value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return sbyte.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out byte value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return byte.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out short value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return short.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out ushort value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return ushort.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out int value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return int.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out uint value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return uint.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out long value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return long.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out ulong value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return ulong.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out float value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return float.TryParse(Arguments[Index++], out value);
		}

		/// <summary>
		/// Parses the next string in the stream as the specified type
		/// </summary>
		/// <param name="value">Resulting instance</param>
		/// <returns><see langword="true"/> if the end of the stream hasn't been reached, and the type could be parsed as the specified type</returns>
		public bool TryReadNext(out double value)
		{
			value = 0;
			if (EndOfStream)
			{
				return false;
			}
			return double.TryParse(Arguments[Index++], out value);
		}
		#endregion
	}
}
