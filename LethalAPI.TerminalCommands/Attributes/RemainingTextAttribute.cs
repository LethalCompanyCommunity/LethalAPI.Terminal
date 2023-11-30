// -----------------------------------------------------------------------
// <copyright file="RemainingTextAttribute.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Attributes;

using System;

/// <summary>
/// Designates that a string argument should contain the remaining text in the command.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class RemainingTextAttribute : Attribute
{
}