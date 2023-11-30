// -----------------------------------------------------------------------
// <copyright file="ParseSentencePatch.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands;

using HarmonyLib;
using Models;
using Patches;
using UnityEngine.Video;

/// <summary>
/// Patches the method used to parse player commands in the terminal, to redirect commands to the <see cref="CommandHandler"/>.
/// </summary>
/// <remarks>
/// Execution may fall back through to the game if no commands are found, or commands let execution fall through.
/// </remarks>
[HarmonyPatch(typeof(Terminal), "ParsePlayerSentence")]
internal static class ParseSentencePatch
{
    [HarmonyPrefix]
    public static bool ParsePrefix(Terminal __instance, ref TerminalNode __state)
    {
        __state = null;
        string commandText = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);

        __state = CommandHandler.TryExecute(commandText, __instance);
        return __state == null;
    }

    [HarmonyPostfix]
    public static TerminalNode ParsePostfix(TerminalNode __result, TerminalNode __state, Terminal __instance)
    {
        if (__state != null)
        {
            TerminalSubmitPatch.LastNode = __state;
            return __state;
        }

        if (__instance.videoPlayer.source == VideoSource.Url)
        {
            __instance.videoPlayer.source = VideoSource.VideoClip;
        }

        TerminalSubmitPatch.LastNode = __result;
        return __result;
    }
}