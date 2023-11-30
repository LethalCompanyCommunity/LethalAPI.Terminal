// -----------------------------------------------------------------------
// <copyright file="StringConverter.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Models;

using System;
using System.Collections.Concurrent;
using System.Reflection;
using Attributes;

/// <summary>
/// Delegate that converts a string to a given object, or thows <see cref="ArgumentException"/> if the value cannot be converted.
/// </summary>
/// <param name="value">String value to convert.</param>
/// <returns></returns>
public delegate object StringConversionHandler(string value);

/// <summary>
/// Provides services for parsing user-entered strings into types, including custom game types.
/// </summary>
public static class StringConverter
{
    /// <summary>
    /// Registry of string converters.
    /// </summary>
    /// <remarks>
    /// Register new converters using <see cref="RegisterFrom{T}(object, bool)"/>.
    /// </remarks>
    public static ConcurrentDictionary<Type, StringConversionHandler> StringConverters { get; } = new ConcurrentDictionary<Type, StringConversionHandler>();

    /// <summary>
    /// Specifies if the default string converters have been registered yet.
    /// </summary>
    private static bool m_Initialized = false;

    /// <summary>
    /// Attempts to convert the specified string to the specified type.
    /// </summary>
    /// <param name="value">String value to parse.</param>
    /// <param name="type">The type to parse the string as.</param>
    /// <param name="result">Resulting object instance, or <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the string could be parsed as the specified type.</returns>
    public static bool TryConvert(string value, Type type, out object result)
    {
        if (!m_Initialized)
        {
            m_Initialized = true;
            RegisterFromType(typeof(DefaultStringConverters), replaceExisting: false);
        }

        if (!StringConverters.TryGetValue(type, out var converter))
        {
            result = null;
            return false;
        }

        try
        {
            result = converter(value);
            return true;
        }
        catch (Exception)
        {
            // Failed to parse as type, return null
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Registers all string converters from a class instance.
    /// </summary>
    /// <remarks>
    /// String converters return any type, have only a string as a parameter, and are decorated with <see cref="StringConverterAttribute"/>.
    /// </remarks>
    /// <typeparam name="T">Type to register from.</typeparam>
    /// <param name="instance">Class instance.</param>
    /// <param name="replaceExisting">When <see langword="true"/>, existing converters for types will be replaced.</param>
    public static void RegisterFrom<T>(T instance, bool replaceExisting = true) where T : class
    {
        RegisterFromType(typeof(T), instance, replaceExisting);
    }

    /// <summary>
    /// Registers all string converters from a class instance or static class.
    /// </summary>
    /// <remarks>
    /// String converters return any type, have only a string as a parameter, and are decorated with <see cref="StringConverterAttribute"/>.
    /// </remarks>
    /// <param name="type">The class type to register from.</param>
    /// <param name="instance">Class instance, or null if the class is static.</param>
    /// <param name="replaceExisting">When <see langword="true"/>, existing converters for types will be replaced.</param>
    public static void RegisterFromType(Type type, object instance = null, bool replaceExisting = true)
    {
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
                (value) => method.Invoke(instance, new object[] { value })
            );

            if (replaceExisting || !StringConverters.ContainsKey(resultingType))
            {
                StringConverters[resultingType] = converter;
            }
        }
    }
}