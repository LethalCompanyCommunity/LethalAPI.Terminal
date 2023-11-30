// -----------------------------------------------------------------------
// <copyright file="TerminalRegistry.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Models;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Attributes;

/// <summary>
/// Manages instances of terminal commands.
/// </summary>
public class TerminalRegistry
{
    /// <summary>
    /// Dictionary containing all registered commands. You shouldn't be interfacing with this directly, instead use the APIs exposed by this class, or <see cref="TerminalModRegistry"/>.
    /// You can enumerate registered commands using <see cref="EnumerateCommands()"/> and <see cref="EnumerateCommands(string)"/>.
    /// </summary>
    private static readonly ConcurrentDictionary<string, List<TerminalCommand>> RegisteredCommands = new (StringComparer.InvariantCultureIgnoreCase);

    /// <summary>
    /// Automatically registers all terminal commands from an instance, and returns a commands <see cref="TerminalModRegistry"/> token, which should be used to deregister all terminal commands when your mod unloads.
    /// </summary>
    /// <typeparam name="T">Instance type.</typeparam>
    /// <param name="instance">Instance to execute commands in.</param>
    /// <returns>Token that can be used to register further commands, and also deregister commands when your mod unloads.</returns>
    public static TerminalModRegistry RegisterFrom<T>(T instance)
        where T : class
    {
        TerminalModRegistry token = new TerminalModRegistry();

        foreach (MethodInfo? method in GetCommandMethods<T>())
        {
            TerminalCommand command = TerminalCommand.FromMethod(method, instance);
            RegisterCommand(command);

            token.Commands.Add(command);
        }

        StringConverter.RegisterFrom(instance);

        return token;
    }

    /// <summary>
    /// Creates a mod-specific terminal command registry, to allow registration and deregistration of commands.
    /// </summary>
    /// <returns>Mod terminal command registry.</returns>
    public static TerminalModRegistry CreateTerminalRegistry()
    {
        return new TerminalModRegistry();
    }

    /// <summary>
    /// Registers a command instance. <see cref="RegisterFrom{T}(T)"/> is preferred. This method is primarily intended for internal use.
    /// </summary>
    /// <param name="command">The terminal command to register.</param>
    public static void RegisterCommand(TerminalCommand command)
    {
        List<TerminalCommand> commands;

        if (!RegisteredCommands.TryGetValue(command.Name, out commands))
        {
            commands = new List<TerminalCommand>();
            RegisteredCommands[command.Name] = commands;
        }

        lock (commands)
        {
            commands.Add(command);
        }
    }

    /// <summary>
    /// De-registers a command instance. You should call <see cref="TerminalModRegistry.Deregister"/> (returned by <see cref="RegisterFrom{T}(T)"/>) instead.
    /// </summary>
    /// <remarks>
    /// Primarily intended for internal use.
    /// </remarks>
    /// <param name="command">Command instance to deregister.</param>
    public static void Deregister(TerminalCommand command)
    {
        if (!RegisteredCommands.TryGetValue(command.Name, out List<TerminalCommand>? overloads))
        {
            return;
        }

        lock (overloads)
        {
            overloads.Remove(command);
        }
    }

    /// <summary>
    /// Fetches all commands by a command name.
    /// </summary>
    /// <param name="commandName">Name of the commands to fetch.</param>
    /// <returns>List of commands.</returns>
    public static IReadOnlyList<TerminalCommand> GetCommands(string commandName)
    {
        if (RegisteredCommands.TryGetValue(commandName, out List<TerminalCommand>? commands))
        {
            return commands;
        }

        return new List<TerminalCommand>();
    }

    /// <summary>
    /// Enumerates registered commands/overloads for a specific command name.
    /// </summary>
    /// <param name="name">Name of the command/s to enumerate.</param>
    /// <returns>Command enumerable.</returns>
    public static IEnumerable<TerminalCommand> EnumerateCommands(string name)
    {
        if (!RegisteredCommands.TryGetValue(name, out List<TerminalCommand>? overloads))
        {
            return Enumerable.Empty<TerminalCommand>();
        }

        return overloads;
    }

    /// <summary>
    /// Enumerates all commands registered to the container.
    /// </summary>
    /// <returns>All terminal command instances.</returns>
    public static IEnumerable<TerminalCommand> EnumerateCommands()
    {
        string[] keys = RegisteredCommands.Keys.ToArray();

        for (int i = 0; i < keys.Length; i++)
        {
            List<TerminalCommand> overloads = RegisteredCommands[keys[i]];

            for (int c = 0; c < overloads.Count; c++)
            {
                yield return overloads[c];
            }
        }
    }

    /// <summary>
    /// Enumerates all methods decorated with <see cref="TerminalCommandAttribute"/> from a type.
    /// </summary>
    /// <typeparam name="T">Type of enumerate command methods from.</typeparam>
    /// <returns>Enumerable of valid terminal command methods.</returns>
    public static IEnumerable<MethodInfo> GetCommandMethods<T>()
    {
        foreach (MethodInfo method in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (method.GetCustomAttribute<TerminalCommandAttribute>() == null)
            {
                continue;
            }

            yield return method;
        }
    }
}