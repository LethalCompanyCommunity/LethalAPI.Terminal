// -----------------------------------------------------------------------
// <copyright file="CommandActivator.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Models;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Attributes;
using Interfaces;
using UnityEngine;

/// <summary>
/// Provides command invoker activation from user input and services.
/// </summary>
public static class CommandActivator
{
    /// <summary>
    /// Attempts to create a command invoker from a MethodInfo.
    /// </summary>
    /// <param name="arguments">User provided arguments.</param>
    /// <param name="services">Services to inject into method parameters.</param>
    /// <param name="method">Method info that represents the terminal command.</param>
    /// <param name="invoker">Resulting invoker for the terminal command.</param>
    /// <returns><see langword="true"/> if the provided arguments and services are sufficient to invoke the command.</returns>
    public static bool TryCreateInvoker(ArgumentStream arguments, ServiceCollection services, MethodInfo method, [NotNullWhen(true)] out Func<object, object>? invoker)
    {
        TerminalCommandAttribute? commandAttribute = method.GetCustomAttribute<TerminalCommandAttribute>();

        ParameterInfo[] parameters = method.GetParameters();

        object[] values = new object[parameters.Length];

        invoker = null;

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo parameter = parameters[i];
            Type type = parameter.ParameterType;

            if (services.TryGetService(type, out object? service))
            {
                values[i] = service;
                continue;
            }
            else if (type == typeof(string) && parameter.GetCustomAttribute<RemainingTextAttribute>() != null)
            {
                if (arguments.TryReadRemaining(out string? remaining))
                {
                    values[i] = remaining;
                    continue;
                }

                return false;
            }

            if (arguments.TryReadNext(type, out object? value))
            {
                values[i] = value;
                continue;
            }

            return false;
        }

        arguments.Reset();
        invoker = (instance) => ExecuteCommand(method, instance, values, commandAttribute?.ClearText ?? false)!;
        return true;
    }

    /// <summary>
    /// Executes this command with the specified arguments.
    /// </summary>
    /// <param name="method">The method to invoke.</param>
    /// <param name="instance">The instance to execute the method with.</param>
    /// <param name="arguments">Arguments used to execute this command. Must precisely match the parameters of the method.</param>
    /// <param name="clearConsole">Indicates whether the console should be cleared after execution.</param>
    /// <returns>Resulting <see cref="TerminalNode"/> response, an <see cref="Interfaces.ITerminalInteraction"/>, or <see langword="null"/>.</returns>
    private static object? ExecuteCommand(MethodInfo method, object instance, object[] arguments, bool clearConsole)
    {
        object result;
        try
        {
            result = method.Invoke(instance, arguments);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error caught while invoking command hander: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return null;
        }

        if (result == null)
        {
            return null;
        }

        Type type = result.GetType();

        if (typeof(TerminalNode).IsAssignableFrom(type))
        {
            return result; // Return manual terminal node response
        }
        else if (typeof(ITerminalInteraction).IsAssignableFrom(type))
        {
            return result; // Return terminal interaction
        }

        // Convert to auto-text response
        TerminalNode response = ScriptableObject.CreateInstance<TerminalNode>();
        response.displayText = result.ToString() + '\n';
        response.clearPreviousText = clearConsole;

        return response;
    }
}