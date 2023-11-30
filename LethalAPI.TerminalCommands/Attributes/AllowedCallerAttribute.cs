// -----------------------------------------------------------------------
// <copyright file="AllowedCallerAttribute.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Attributes;

using System;

using Models.Enums;

/// <summary>
/// Specifies what permission level is required for this command.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AllowedCallerAttribute : AccessControlAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AllowedCallerAttribute"/> class.
    /// Sets the required permission level for the command.
    /// </summary>
    /// <param name="caller">Permission level.</param>
    public AllowedCallerAttribute(AllowedCaller caller)
    {
        Caller = caller;
    }

    /// <summary>
    /// Gets the required permission level.
    /// </summary>
    public AllowedCaller Caller { get; }

    /// <summary>
    /// Checks if the local player has permission to execute the command.
    /// </summary>
    /// <returns><see langword="true"/> if the player has permission to execute the command. </returns>
    public override bool CheckAllowed()
    {
        switch (Caller)
        {
            case AllowedCaller.None:
                return false;

            case AllowedCaller.Player:
                return true;

            case AllowedCaller.Host:
                if (StartOfRound.Instance == null)
                {
                    return false;
                }

                return StartOfRound.Instance.IsHost;
            default:
                return true;
        }
    }
}