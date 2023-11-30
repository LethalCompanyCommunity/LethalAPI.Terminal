// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands;

using System;

using Commands;
using Core;
using HarmonyLib;
using Models;

/// <summary>
///     The main plugin class.
/// </summary>
public class Plugin : LethalAPI.Core.Features.Plugin<Config>
{
    /// <summary>
    ///     Gets the main <see cref="Plugin"/> instance.
    /// </summary>
    public static Plugin Instance { get; private set; } = null!;

    /// <summary>
    ///     Gets the main <see cref="Harmony"/> instance for the plugin.
    /// </summary>
    public Harmony HarmonyInstance => new("com.LethalAPI.TerminalCommands");

    /// <inheritdoc />
    public override string Name => "LethalAPI.TerminalCommands";

    /// <inheritdoc />
    public override string Description => "A library that allows the creation of custom terminal commands for Lethal Company mods";

    /// <inheritdoc />
    public override string Author => "LethalAPI Modding Community";

    /// <inheritdoc />
    public override Version Version => new(1, 0, 0);

    /// <summary>
    /// Gets the main terminal registry instance.
    /// </summary>
    internal TerminalModRegistry Terminal { get; private set; } = null!;

    /// <inheritdoc />
    public override void OnEnabled()
    {
        if (!this.Config.IsEnabled)
        {
            return;
        }

        Instance = this;
        Log.Debug($"Installing Patches");
        HarmonyInstance.PatchAll(typeof(Plugin).Assembly);
        Log.Debug($"Registering built-in Commands");

        // Create registry for the Terminals API
        this.Terminal = TerminalRegistry.CreateTerminalRegistry();

        // Register commands, don't care about the instance
        this.Terminal.RegisterFrom<CommandInfoCommands>();

        Log.Info($"Started plugin &3LethalAPI.TerminalCommands &gv1.0.0 &rby &7LethalAPI Modding Community&r.{(this.Config.Debug ? " [&2Debug&r]" : string.Empty)}");
    }
}