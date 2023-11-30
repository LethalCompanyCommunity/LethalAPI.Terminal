// -----------------------------------------------------------------------
// <copyright file="CommandComparer.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Models;

using System.Collections.Generic;

/// <summary>
/// Compares terminal commands to determine preference order. Orders by priority, then parameter count.
/// </summary>
public class CommandComparer : IComparer<TerminalCommand>
{
    /// <inheritdoc />
    public int Compare(TerminalCommand x, TerminalCommand y)
    {
        if (x.Priority > y.Priority)
        {
            return 1;
        }
        else if (x.Priority < y.Priority)
        {
            return -1;
        }

        return x.ArgumentCount.CompareTo(y.ArgumentCount);
    }
}