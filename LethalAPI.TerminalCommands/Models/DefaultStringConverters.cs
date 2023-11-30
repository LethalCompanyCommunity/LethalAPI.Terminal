// -----------------------------------------------------------------------
// <copyright file="DefaultStringConverters.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Models;

using System;
using System.Linq;

using Attributes;
using GameNetcodeStuff;

/// <summary>
/// Contains the default built-in string type converters.
/// </summary>
public static class DefaultStringConverters
{
    [StringConverter]
    public static string ParseString(string input)
    {
        return input;
    }

    [StringConverter]
    public static sbyte ParseSByte(string input)
    {
        if (sbyte.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static byte ParseByte(string input)
    {
        if (byte.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static short ParseShort(string input)
    {
        if (short.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static ushort ParseUShort(string input)
    {
        if (ushort.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static int ParseInt(string input)
    {
        if (int.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static uint ParseUInt(string input)
    {
        if (uint.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static long ParseLong(string input)
    {
        if (long.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static ulong ParseULong(string input)
    {
        if (ulong.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static float ParseFloat(string input)
    {
        if (float.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static double ParseDouble(string input)
    {
        if (double.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static decimal ParseDecimal(string input)
    {
        if (decimal.TryParse(input, out var value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    [StringConverter]
    public static PlayerControllerB ParsePlayerControllerB(string value)
    {
        if (StartOfRound.Instance == null)
        {
            throw new ArgumentException("Game has not started");
        }

        PlayerControllerB player = null;
        if (ulong.TryParse(value, out var steamID))
        {
            player = StartOfRound.Instance.allPlayerScripts
                .FirstOrDefault(x => x.playerSteamId == steamID);
        }

        if (player == null)
        {
            player = StartOfRound.Instance.allPlayerScripts
                .FirstOrDefault(x => x.playerUsername.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1);
        }

        if (player == null)
        {
            throw new ArgumentException("Failed to find player");
        }

        return player;
    }
}