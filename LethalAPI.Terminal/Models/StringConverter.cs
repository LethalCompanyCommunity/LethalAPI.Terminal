using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using LethalAPI.LibTerminal.Attributes;

namespace LethalAPI.LibTerminal.Models
{
	/// <summary>
	/// Delegate that converts a string to a given object, or throws <seealso cref="ArgumentException"/> if the value cannot be converted
	/// </summary>
	/// <param name="value">String value to convert</param>
	/// <returns></returns>
	public delegate object StringConversionHandler(string value);

	/// <summary>
	/// Provides services for parsing user-entered strings into types, including custom game types.
	/// </summary>
	public static class StringConverter
	{
		/// <summary>
		/// Registry of string converters
		/// </summary>
		/// <remarks>
		/// Register new converters using <seealso cref="RegisterFrom{T}(object, bool)"/>
		/// </remarks>
		private static ConcurrentDictionary<Type, List<StringConversionHandler>> m_StringConverters { get; } = new ConcurrentDictionary<Type, List<StringConversionHandler>>();

		/// <summary>
		/// Specifies if the default string converters have been registered yet
		/// </summary>
		private static bool m_Initialized = false;

		/// <summary>
		/// Attempts to convert the specified string to the specified type
		/// </summary>
		/// <param name="value">String value to parse</param>
		/// <param name="type">The type to parse the string as</param>
		/// <param name="result">Resulting object instance, or <see langword="null"/></param>
		/// <returns><see langword="true"/> if the string could be parsed as the specified type</returns>
		public static bool TryConvert(string value, Type type, out object? result)
		{
			if (!m_Initialized)
			{
				m_Initialized = true;
				RegisterFromType(typeof(DefaultStringConverters));
			}

			if (!m_StringConverters.TryGetValue(type, out var converters))
			{
				result = null;
				return false;
			}

			for (int i = 0; i < converters.Count; i++)
			{
				try
				{
					result = converters[i](value);
					return true;
				}
				catch (Exception)
				{
					// Failed to parse as type, try next handler
				}
			}

			result = null;
			return false;
		}

		/// <summary>
		/// Registers all string converters from a class instance
		/// </summary>
		/// <remarks>
		/// String converters return any type, have only a string as a parameter, and are decorated with <seealso cref="StringConverterAttribute"/>.
		/// </remarks>
		/// <typeparam name="T">Type to register from</typeparam>
		/// <param name="instance">Class instance</param>
		/// <param name="replaceExisting">When <see langword="true"/>, existing converters for types will be replaced</param>
		public static List<RegisteredStringConverter> RegisterFrom<T>(T instance) where T : class
		{
			return RegisterFromType(typeof(T), instance);
		}

		/// <summary>
		/// Deregisters a string converter by it's type, with the specified converter instance.
		/// </summary>
		/// <param name="converter">The string converter to de-register</param>
		/// <returns><see langword="true"/> if the handler could be removed, <see langword="false"/> otherwise</returns>
		public static bool Deregister(RegisteredStringConverter converter)
		{
			if (!m_StringConverters.TryGetValue(converter.Type, out var converters))
			{
				return false;
			}

			return converters.Remove(converter.Handler);
		}

		/// <summary>
		/// Checks if a handler is available to convert to the specified type
		/// </summary>
		/// <param name="type">The desired type to convert strings to</param>
		/// <returns><see langword="true"/> if a string converter is available for the specified type</returns>
		public static bool CanConvert(Type type)
		{
			return m_StringConverters.ContainsKey(type);
		}

		/// <summary>
		/// Registers all string converters from a class instance or static class
		/// </summary>
		/// <remarks>
		/// String converters return any type, have only a string as a parameter, and are decorated with <seealso cref="StringConverterAttribute"/>.
		/// </remarks>
		/// <param name="type">The class type to register from</param>
		/// <param name="instance">Class instance, or null if the class is static</param>
		public static List<RegisteredStringConverter> RegisterFromType(Type type, object? instance = null)
		{
			var results = new List<RegisteredStringConverter>();
			foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
			{
				if (method.GetCustomAttribute<StringConverterAttribute>() == null)
				{
					continue;
				}

				var parameters = method.GetParameters();

				if (parameters.Length != 1)
				{
					continue;
				}

				if (parameters[0].ParameterType != typeof(string))
				{
					continue;
				}

				var resultingType = method.ReturnType;

				var converter = new StringConversionHandler(
					(value) => method.Invoke(instance, [value])
				);

				if (!m_StringConverters.TryGetValue(resultingType, out var handlers))
				{
					handlers = new List<StringConversionHandler>();
					m_StringConverters[resultingType] = handlers;
				}

				handlers.Add(converter);

				results.Add(new RegisteredStringConverter(resultingType, converter));
			}

			return results;
		}
	}
}