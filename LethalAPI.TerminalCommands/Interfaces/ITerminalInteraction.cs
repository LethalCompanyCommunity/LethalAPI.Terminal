// -----------------------------------------------------------------------
// <copyright file="ITerminalInteraction.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Interfaces;

using Models;

/// <summary>
/// The base interface for all terminal interactions.
/// </summary>
/// <remarks>
/// <para>
/// You can find general implementations of this under the interactions namespace (see <see cref="Interactions.ConfirmInteraction"/> and <see cref="Interactions.TerminalInteraction"/>).
/// </para>
/// <para>
/// You can also implement this yourself to implement custom interaction patterns and handlers.
/// </para>
/// </remarks>
public interface ITerminalInteraction
{
    /// <summary>
    /// Gets the prompt displayed to the user when this interaction is returned from a command.
    /// </summary>
    /// <remarks>
    /// This acts as the command response, and prompt for further information.
    /// </remarks>
    TerminalNode Prompt { get; }

    /// <summary>
    /// Gets the service collection that command context will be registered to.
    /// </summary>
    /// <remarks>
    /// You can use this to inject services into your own handler delegates, or fetch command context such as the <see cref="Terminal"/> instance when <see cref="HandleTerminalResponse(ArgumentStream)"/> is being run.
    /// </remarks>
    ServiceCollection Services { get; }

    /// <summary>
    /// The handler for the terminal interaction. This method receives the next line of input from the terminal.
    /// </summary>
    /// <remarks>
    /// You can return <see langword="null"/> to yield execution to either a parent interaction, or the command handler.
    /// </remarks>
    /// <param name="arguments">Arguments provided by the user. This is the full argument input, including the first word that is commonly taken as the command name.</param>
    /// <returns>A <see cref="TerminalNode"/>, another <see cref="ITerminalInteraction"/>, an object that represents the response text from this interaction, or <see langword="null"/>.</returns>
    object? HandleTerminalResponse(ArgumentStream arguments);
}