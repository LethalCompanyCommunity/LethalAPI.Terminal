using BepInEx.Logging;
using HarmonyLib;

namespace LethalAPI.TerminalCommands.Patches
{
	[HarmonyPatch(typeof(Terminal), "PlayTerminalAudioClientRpc")]
	public static class DebugPatch
	{
		private static readonly ManualLogSource m_Logger = new ManualLogSource("LethalAPI.TerminalCommands:Debug");

		[HarmonyPrefix]
		public static void Prefix(int clipIndex)
		{
			m_Logger.LogInfo($"Terminal playing clip: {clipIndex}");
		}
	}
}
