// -----------------------------------------------------------------------
// <copyright file="StringConverterAttribute.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Attributes;

using System;

/// <summary>
/// Used to mark a method as a string converter.
/// </summary>
/// <remarks>
/// String converters should take only a single argument of a string, and return the type they convert strings to.
/// <para>
/// String Converter methods can throw <see cref="ArgumentException"/> if the input string is in the incorrect format.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class StringConverterAttribute : Attribute
{
}