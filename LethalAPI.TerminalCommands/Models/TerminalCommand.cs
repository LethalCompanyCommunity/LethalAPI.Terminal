// -----------------------------------------------------------------------
// <copyright file="TerminalCommand.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable MemberCanBePrivate.Global
namespace LethalAPI.TerminalCommands.Models;

using System;
using System.Collections.Generic;
using System.Reflection;

using Attributes;

/// <summary>
/// Command instance representing a registered terminal command.
/// </summary>
public class TerminalCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TerminalCommand"/> class.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="method">The method to execute when the command is run.</param>
    /// <param name="instance">The instance to pass on when the command is run.</param>
    /// <param name="clearConsole">Indicates whether the console be cleared after execution.</param>
    /// <param name="syntax">The syntax of the command.</param>
    /// <param name="description">A description of the command.</param>
    /// <param name="priority">The run priority of the command.</param>
    public TerminalCommand(string name, MethodInfo method, object instance, bool clearConsole, string? syntax = null, string? description = null, int priority = 0)
    {
        this.Name = name;
        this.Method = method;
        this.Instance = instance;
        this.ClearConsole = clearConsole;
        this.ArgumentCount = method.GetParameters().Length;
        this.Syntax = syntax;
        this.Description = description;
        this.Priority = priority;
    }

    /// <summary>
    /// Gets the base name of the command.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets source method the command is declared in.
    /// </summary>
    public MethodInfo Method { get; }

    /// <summary>
    /// Gets instance of the <see cref="Method"/>'s parent class.
    /// </summary>
    public object Instance { get; }

    /// <summary>
    /// Gets a value indicating whether when true, the console is cleared before writing the command response.
    /// </summary>
    public bool ClearConsole { get; }

    /// <summary>
    /// Gets number of arguments used by this command.
    /// </summary>
    public int ArgumentCount { get; }

    /// <summary>
    /// Gets optional command syntax.
    /// </summary>
    public string? Syntax { get; }

    /// <summary>
    /// Gets optional command description. This value being set enrolls the command to be shown in help commands.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Gets command execution priority, with a default of 0.
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Parses a command from an instance, optionally overriding the command name.
    /// </summary>
    /// <param name="info">Method info that declares the command.</param>
    /// <param name="instance">Object instance to execute the command in.</param>
    /// <param name="overrideName">Optional command name override.</param>
    /// <returns>Terminal command instance.</returns>
    public static TerminalCommand FromMethod(MethodInfo info, object instance, string? overrideName = null)
    {
        bool clear = false;
        string? syntax = null;
        string? description = null;
        string? name = overrideName;
        int priority = 0;

        TerminalCommandAttribute? command = info.GetCustomAttribute<TerminalCommandAttribute>();
        if (command != null)
        {
            name ??= command.CommandName;
            clear = command.ClearText;
        }

        CommandInfoAttribute? commandInfo = info.GetCustomAttribute<CommandInfoAttribute>();
        if (commandInfo != null)
        {
            syntax = commandInfo.Syntax;
            description = commandInfo.Description;
        }

        CommandPriority? priorityValue = info.GetCustomAttribute<CommandPriority>();
        if (priorityValue != null)
        {
            priority = priorityValue.Priority;
        }

        return new TerminalCommand(name!, info, instance, clear, syntax, description, priority);
    }

    /// <summary>
    /// Checks if the local player has permission to execute this command.
    /// </summary>
    /// <returns><see langword="true"/> if the player can execute the command.</returns>
    public bool CheckAllowed()
    {
        IEnumerable<AccessControlAttribute> accessControl = Method.GetCustomAttributes<AccessControlAttribute>();

        foreach (AccessControlAttribute? attribute in accessControl)
        {
            if (!attribute.CheckAllowed())
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Attempts to parse a user-entered argument list, to create an invoker for a command.
    /// </summary>
    /// <param name="arguments">Arguments to use when executing the command.</param>
    /// <param name="services">The light-weight temporal service collection to provide services for command execution.</param>
    /// <param name="invoker">Delegate that executes the command using the specified arguments.</param>
    /// <returns><see langword="true"/> if the provided arguments match the signature for this command, and could be parsed correctly.</returns>
    public bool TryCreateInvoker(ArgumentStream arguments, ServiceCollection services, out Func<object>? invoker)
    {
        arguments.Reset();

        if (CommandActivator.TryCreateInvoker(arguments, services, Method, out Func<object, object> activatedInvoker))
        {
            invoker = () => activatedInvoker(Instance);
            return true;
        }

        invoker = null;
        return false;
    }
}