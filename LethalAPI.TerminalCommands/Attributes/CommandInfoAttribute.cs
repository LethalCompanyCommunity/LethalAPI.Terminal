// -----------------------------------------------------------------------
// <copyright file="CommandInfoAttribute.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Attributes;

using System;

/// <summary>
/// Decorates a terminal command with command info.
/// </summary>
/// <remarks>
/// This attribute registers the command to be listed in the help/other command list.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandInfoAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandInfoAttribute"/> class.
    /// Registers a terminal command to be listed in the help/other command list.
    /// </summary>
    /// <param name="syntax">The usage pattern of the command.</param>
    /// <param name="description">A short description of what the command does.</param>
    /// <remarks>
    /// <paramref name="syntax"/> should look something like "[Target Player] [Message...]".
    /// </remarks>
    public CommandInfoAttribute(string description, string syntax = "")
    {
        Syntax = syntax;
        Description = description;
    }

    /// <summary>
    /// Gets the syntax/usage of the command.
    /// </summary>
    public string Syntax { get; }

    /// <summary>
    /// Gets a short description of what the command does.
    /// </summary>
    public string Description { get; }
}