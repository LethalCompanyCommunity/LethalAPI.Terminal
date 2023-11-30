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
    /// <summary>
    /// Parses a string and returns the value as a <see cref="string"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="string"/> value.</returns>
    [StringConverter]
    public static string ParseString(string input)
    {
        return input;
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="sbyte"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="sbyte"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static sbyte ParseSByte(string input)
    {
        if (sbyte.TryParse(input, out sbyte value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="byte"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="byte"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static byte ParseByte(string input)
    {
        if (byte.TryParse(input, out byte value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="short"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="short"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static short ParseShort(string input)
    {
        if (short.TryParse(input, out short value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="ushort"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="ushort"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static ushort ParseUShort(string input)
    {
        if (ushort.TryParse(input, out ushort value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as an <see cref="int"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="int"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static int ParseInt(string input)
    {
        if (int.TryParse(input, out int value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="uint"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="uint"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static uint ParseUInt(string input)
    {
        if (uint.TryParse(input, out uint value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="long"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="long"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static long ParseLong(string input)
    {
        if (long.TryParse(input, out long value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="ulong"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="ulong"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static ulong ParseULong(string input)
    {
        if (ulong.TryParse(input, out ulong value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="float"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="float"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static float ParseFloat(string input)
    {
        if (float.TryParse(input, out float value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="double"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="double"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static double ParseDouble(string input)
    {
        if (double.TryParse(input, out double value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="decimal"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="decimal"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static decimal ParseDecimal(string input)
    {
        if (decimal.TryParse(input, out decimal value))
        {
            return value;
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Parses a string and returns the value as a <see cref="PlayerControllerB"/>.
    /// </summary>
    /// <param name="input">The string input to parse.</param>
    /// <returns>The parsed <see cref="PlayerControllerB"/> value.</returns>
    /// <exception cref="ArgumentException">Thrown if the input is invalid for this converter.</exception>
    [StringConverter]
    public static PlayerControllerB ParsePlayerControllerB(string input)
    {
        if (StartOfRound.Instance == null)
        {
            throw new ArgumentException("Game has not started");
        }

        PlayerControllerB player = null;
        if (ulong.TryParse(input, out ulong steamID))
        {
            player = StartOfRound.Instance.allPlayerScripts.FirstOrDefault(x => x.playerSteamId == steamID);
        }

        if (player == null)
        {
            player = StartOfRound.Instance.allPlayerScripts.FirstOrDefault(x => x.playerUsername.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) != -1);
        }

        if (player == null)
        {
            throw new ArgumentException("Failed to find player");
        }

        return player;
    }
}