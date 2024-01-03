using HarmonyLib;
using LethalAPI.TerminalCommands.Helpers;
using LethalAPI.TerminalCommands.Models;
namespace LethalAPI.TerminalCommands.Patches
{
	/// <summary>
	/// Loads the splash screen node of the current <seealso cref="Interfaces.ITerminalInterface"/>, so long as one is active.
	/// </summary>
	[HarmonyPatch(typeof(Terminal), "BeginUsingTerminal")]
	internal static class BeginUsingTerminalPatch
	{
        private static TimeHelper timeHelper;

        [HarmonyPostfix]
		public static void Postfix(Terminal __instance)
		{
            //timeHelper ??= new TimeHelper();

            var terminalInterface = CommandHandler.CurrentInterface;
			if (terminalInterface == null)
			{
				return;
			}

			var splashScreen = terminalInterface.GetSplashScreen(__instance);

			if (splashScreen != null)
			{
				__instance.LoadNewNode(splashScreen);
			}
		}
	}
}
