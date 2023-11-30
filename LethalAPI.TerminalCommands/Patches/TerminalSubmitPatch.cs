// -----------------------------------------------------------------------
// <copyright file="TerminalSubmitPatch.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable UnusedMember.Local
// ReSharper disable StringLiteralTypo
#pragma warning disable SA1313 // parameter name shouldn't begin with a '_'.
namespace LethalAPI.TerminalCommands.Patches;

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using HarmonyLib;
using UnityEngine;

/// <summary>
/// Patches the submit method of the Terminal to modify its auto-scroll behavior.
/// </summary>
/// <remarks>
/// By default, the game always scrolls to the top on command execution, this patch makes it so it only scrolls to the top on terminal clearance.
/// </remarks>
[HarmonyPatch(typeof(Terminal), nameof(Terminal.OnSubmit))]
internal static class TerminalSubmitPatch
{
    /// <summary>
    /// Gets or sets set by the <see cref="ParseSentencePatch"/>, to allow the postfix to access the last parsed node.
    /// </summary>
    internal static TerminalNode? LastNode { get; set; }

    [HarmonyPrefix]
    private static void Prefix()
    {
        LastNode = null;
    }

    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        CodeInstruction[] code = instructions.ToArray();

        // Backward scan for the reference to Select()
        for (int i = code.Length - 1; i >= 0; i--)
        {
            if (code[i].opcode != OpCodes.Callvirt)
            {
                continue;
            }

            if (code[i + 1].opcode != OpCodes.Ldarg_0)
            {
                ReportTranspileError("Ldarg_0 expected after final callVirt, not found");
                return code;
            }

            code[i + 1] = new CodeInstruction(OpCodes.Ret);
            return code;
        }

        ReportTranspileError("Failed to find Callvirt in backward scan");

        return code;
    }

    private static void ReportTranspileError(string message)
    {
        Log.Error($"Failed to transpile OnSubmit to remove Scroll To Bottom. Did the method get modified in an update? ({message})");
        Log.Warn("This won't break the mod, but it will cause some odd terminal scrolling behavior");
    }

    [HarmonyPostfix]
    private static void Postfix(Terminal __instance, ref Coroutine? ___forceScrollbarCoroutine)
    {
        if (LastNode == null || LastNode.clearPreviousText)
        {
            ExecuteScrollCoroutine(__instance, ref ___forceScrollbarCoroutine);
            return;
        }

        __instance.StartCoroutine(nameof(Terminal.forceScrollbarDown));
    }

    private static void ExecuteScrollCoroutine(Terminal terminal, ref Coroutine? forceScrollbarCoroutine)
    {
        if (forceScrollbarCoroutine != null)
        {
            terminal.StopCoroutine(forceScrollbarCoroutine);
        }

        forceScrollbarCoroutine = terminal.StartCoroutine(nameof(Terminal.forceScrollbarUp));
    }
}