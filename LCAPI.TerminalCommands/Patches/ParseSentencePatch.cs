using LCAPI.TerminalCommands.Models;
using LCAPI.TerminalCommands.Patches;
using HarmonyLib;
using UnityEngine.Video;

namespace LCAPI.TerminalCommands
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
}
