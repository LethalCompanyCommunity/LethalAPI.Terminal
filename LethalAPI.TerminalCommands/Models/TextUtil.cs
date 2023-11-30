// -----------------------------------------------------------------------
// <copyright file="TextUtil.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Models;

/// <summary>
/// Provides some misc string manipulation tools.
/// </summary>
public static class TextUtil
{
    /// <summary>
    /// Applies a minimum trailing character count to the end of a string, padding the end with the specified character until it reaches the specified number of trailing characters.
    /// </summary>
    /// <param name="text">Text to apply minimum padding to.</param>
    /// <param name="character">The character to check the text for trailing instances of, and to append to the text as needed.</param>
    /// <param name="minPadding">Minimum number trailing instances of <paramref name="character"/>.</param>
    /// <returns>Resulting string with the minimum padding applied.</returns>
    public static string SetEndPadding(string text, char character, int minPadding)
    {
        string result = text;
        int newlineCount = 0;

        for (int i = text.Length - 1; i >= 0; i--)
        {
            if (text[i] == character)
            {
                newlineCount++;
            }
            else
            {
                break;
            }
        }

        int newlinesRequired = minPadding - newlineCount;

        if (newlinesRequired > 0)
        {
            result += new string(character, newlinesRequired);
        }

        return result;
    }
}