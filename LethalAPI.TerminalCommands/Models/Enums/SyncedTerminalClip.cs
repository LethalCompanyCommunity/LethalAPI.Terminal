// -----------------------------------------------------------------------
// <copyright file="SyncedTerminalClip.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Models.Enums;

/// <summary>
/// Provides a list of audio effects that can be played from a <see cref="TerminalNode"/>.
/// </summary>
public enum SyncedTerminalClip
{
    /// <summary>
    /// Plays a random item purchased sound effect.
    /// </summary>
    /// <remarks>
    /// Network synced audio effect.
    /// </remarks>
    ItemPurchased = 0,

    /// <summary>
    /// The Error / Unknown Command sound effect.
    /// </summary>
    /// <remarks>
    /// Network synced audio effect.
    /// </remarks>
    Error = 1,

    /// <summary>
    /// Plays the telegraph-like broadcast effect played when loading some log entries.
    /// </summary>
    /// <remarks>
    /// Network synced audio effect.
    /// </remarks>
    BroadcastEffect = 2,
}