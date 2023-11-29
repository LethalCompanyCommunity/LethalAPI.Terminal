using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace LethalAPI.TerminalCommands.Patches
{
	/// <summary>
	/// Decreases the delay after entering the terminal before you can type by 80%
	/// </summary>
	[HarmonyPatch(typeof(Terminal), "selectTextFieldDelayed")]
	internal static class SelectTextFieldPatch
	{
		[HarmonyPrefix]
		public static bool Prefix()
		{
			return false;
		}

		[HarmonyPostfix]
		public static void Postfix(Terminal __instance, ref IEnumerator __result)
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
}
