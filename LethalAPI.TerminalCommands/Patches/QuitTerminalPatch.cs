using HarmonyLib;
namespace LethalAPI.TerminalCommands.Patches
{
	[HarmonyPatch(typeof(Terminal), "QuitTerminal")]
	internal static class QuitTerminalPatch
	{
        [HarmonyPostfix]
		public static void Postfix(Terminal __instance)
		{
			//new TimeHelper().StopUpdating();
		}
	}
}
