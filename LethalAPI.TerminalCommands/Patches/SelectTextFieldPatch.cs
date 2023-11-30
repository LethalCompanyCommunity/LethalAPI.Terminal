// -----------------------------------------------------------------------
// <copyright file="SelectTextFieldPatch.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.TerminalCommands.Patches;

using System.Collections;

using HarmonyLib;
using UnityEngine;

/// <summary>
/// Decreases the delay after entering the terminal before you can type by 80%.
/// </summary>
[HarmonyPatch(typeof(Terminal), nameof(Terminal.selectTextFieldDelayed))]
internal static class SelectTextFieldPatch
{
    [HarmonyPrefix]
    private static bool Prefix()
    {
        return false;
    }

    [HarmonyPostfix]
    private static void Postfix(Terminal __instance, ref IEnumerator __result)
    {
        __result = Patch(__instance);
    }

    private static IEnumerator Patch(Terminal terminal)
    {
        yield return new WaitForSeconds(0.2f);
        terminal.screenText.ActivateInputField();
        terminal.screenText.Select();
        yield break;
    }
}