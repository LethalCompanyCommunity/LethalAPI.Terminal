using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace LethalAPI.LibTerminal.Patches
{
	/// <summary>
	/// Patches the submit method of the Terminal to modify its auto-scroll behaviour
	/// </summary>
	/// <remarks>
	/// By default, the game always scrolls to the top on command execution, this patch makes it so it only scrolls to the top on terminal clearance
	/// </remarks>
	[HarmonyPatch(typeof(Terminal), "OnSubmit")]
	internal static class TerminalSubmitPatch
	{
		/// <summary>
		/// Set by the <seealso cref="ParseSentencePatch"/>, to allow the postfix to access the last parsed node
		/// </summary>
		public static TerminalNode LastNode { get; set; }

		private static ManualLogSource m_LogSource = new ManualLogSource("LethalAPI.Terminal");

		[HarmonyPrefix]
		public static void Prefix()
		{
			LastNode = null;
		}

		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var code = instructions.ToArray();

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
			m_LogSource.LogError($"Failed to transpile OnSubmit to remove Scroll To Bottom. Did the method get modified in an update? ({message})");
			m_LogSource.LogWarning("This won't break the mod, but it will cause some odd terminal scrolling behavior");
		}

		[HarmonyPostfix]
		public static void Postfix(Terminal __instance, ref Coroutine ___forceScrollbarCoroutine)
		{
			if (LastNode == null || LastNode.clearPreviousText)
			{
				ExecuteScrollCoroutine(__instance, ref ___forceScrollbarCoroutine);
				return;
			}

			__instance.StartCoroutine("forceScrollbarDown");
		}

		private static void ExecuteScrollCoroutine(Terminal terminal, ref Coroutine forceScrollbarCoroutine)
		{
			if (forceScrollbarCoroutine != null)
			{
				terminal.StopCoroutine(forceScrollbarCoroutine);
			}

			forceScrollbarCoroutine = terminal.StartCoroutine("forceScrollbarUp");
		}
	}
}
