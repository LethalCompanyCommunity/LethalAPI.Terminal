using HarmonyLib;
using LethalAPI.TerminalCommands.Helpers;
namespace LethalAPI.TerminalCommands.Patches
{
	[HarmonyPatch(typeof(StartOfRound), "StartGame")]
	internal static class StartGamePatch
	{
        [HarmonyPostfix]
		public static void Postfix(StartOfRound __instance)
		{
            if (MainThreadDispatcher.Instance != null)
                return; // Already initialized

            MainThreadDispatcher.Initialize();
            UnityEngine.Object.DontDestroyOnLoad(MainThreadDispatcher.Instance);
        }
    }
}
