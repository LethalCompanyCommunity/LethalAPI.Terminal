using System;
using BepInEx.Logging;
using CustomTerminalCommands.Models;
using CustomTerminalCommands.Patches;
using HarmonyLib;
using UnityEngine.Video;

namespace CustomTerminalCommands
{
	/// <summary>
	/// Patches the method used to parse player commands in the terminal, to redirect commands to the <seealso cref="CommandHandler"/>
	/// </summary>
	/// <remarks>
	/// Execution may fall back through to the game if no commands are found, or commands let execution fall through
	/// </remarks>
	[HarmonyPatch(typeof(Terminal), "ParsePlayerSentence")]
	public static class ParseSentencePatch
	{

		[HarmonyPrefix]
		public static bool ParsePrefix(Terminal __instance, ref TerminalNode __state)
		{
			__state = null;
			var commandText = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);

            Console.WriteLine($"Handle execute command: {commandText}");
            __state = CommandHandler.TryExecute(commandText, __instance);
            Console.WriteLine($"HasResult: {__state != null}");
            return __state == null;
		}

		[HarmonyPostfix]
		public static TerminalNode ParsePostfix(TerminalNode __result, TerminalNode __state, Terminal __instance)
		{
			if (__state != null)
			{
                Console.WriteLine($"Replacing terminal node");
                TerminalSubmitPatch.LastNode = __state;
				return __state;
			}

			Console.WriteLine($"Replacing falling through to game");

			if (__instance.videoPlayer.source == VideoSource.Url)
			{
				__instance.videoPlayer.source = VideoSource.VideoClip;
			}

			TerminalSubmitPatch.LastNode = __result;
			return __result;
		}
	}
}
