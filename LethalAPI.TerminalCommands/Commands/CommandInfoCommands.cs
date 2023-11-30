// -----------------------------------------------------------------------
// <copyright file="CommandInfoCommands.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Commands;

using System.Linq;
using System.Text;

using Attributes;
using Models;

/// <summary>
/// Contains Terminal Command definitions for the built-in help commands.
/// </summary>
public class CommandInfoCommands
{
    /// <summary>
    /// A terminal command for viewing other commands.
    /// </summary>
    /// <returns>The resulting string.</returns>
    [TerminalCommand("Other", clearText: true)]
    public string CommandList()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("Other commands:");
        builder.AppendLine();
        builder.AppendLine(">VIEW MONITOR");
        builder.AppendLine("To toggle on/off the main monitor's map cam");
        builder.AppendLine();
        builder.AppendLine(">SWITCH {RADAR}");
        builder.AppendLine("To switch the player view on the main monitor");
        builder.AppendLine();
        builder.AppendLine(">PING [Radar booster name]");
        builder.AppendLine("To switch the player view on the main monitor");
        builder.AppendLine();
        builder.AppendLine(">SCAN");
        builder.AppendLine("To scan for the number of items left on the current planet");
        builder.AppendLine();

        foreach (TerminalCommand? command in TerminalRegistry.EnumerateCommands())
        {
            if (command.Description == null)
            {
                continue;
            }

            if (!command.CheckAllowed())
            {
                continue;
            }

            builder.AppendLine($">{command.Name.ToUpper()} {command.Syntax?.ToUpper()}");
            builder.AppendLine(command.Description);
            builder.AppendLine();
        }

        return builder.ToString();
    }

    /// <summary>
    /// A terminal command for viewing other commands.
    /// </summary>
    /// <param name="name">The name of the command to get help for.</param>
    /// <returns>The resulting string.</returns>
    [TerminalCommand("Help", clearText: false)]
    [CommandInfo("Shows further information about a command", "[Command]")]
    public string HelpCommand(string name)
    {
        StringBuilder builder = new StringBuilder();
        TerminalCommand[] commands = TerminalRegistry.EnumerateCommands(name).ToArray();

        if (commands.Length == 0)
        {
            return $"Unknown command: '{name}'";
        }

        foreach (TerminalCommand command in commands)
        {
            builder.AppendLine($">{command.Name.ToUpper()} {command.Syntax?.ToUpper()}");
            builder.AppendLine(command.Description);
            if (!command.CheckAllowed())
            {
                builder.AppendLine("[Host Only]");
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}