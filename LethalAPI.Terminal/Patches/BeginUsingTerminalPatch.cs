using HarmonyLib;
using LethalAPI.LibTerminal.Models;
namespace LethalAPI.LibTerminal.Patches
{
	/// <summary>
	/// Loads the splash screen node of the current <seealso cref="Interfaces.ITerminalInterface"/>, so long as one is active.
	/// </summary>
	[HarmonyPatch(typeof(Terminal), "BeginUsingTerminal")]
	internal static class BeginUsingTerminalPatch
	{
		[HarmonyPostfix]
		public static void Postfix(Terminal __instance)
		{
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
