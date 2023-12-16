using HarmonyLib;
using LethalAPI.TerminalCommands.Interfaces;
using LethalAPI.TerminalCommands.Models;

namespace LethalAPI.TerminalCommands.Patches
{
	/// <summary>
	/// <para>
	/// Removes trailing newlines from the start of terminal responses.
	/// </para>
	/// <para>
	/// Also executes pre/post text processing as specified by the current <seealso cref="ITerminalInterface"/>
	/// </para>
	/// </summary>
	/// <remarks>
	/// This is to address newlines that the game adds to the start of all terminal responses, that slowly forces the terminal window down and off-screen
	/// </remarks>
	[HarmonyPatch(typeof(Terminal), "TextPostProcess")]
	internal static class TextPostProcessPatch
	{
		[HarmonyPrefix]
		public static bool Prefix(Terminal __instance, ref string modifiedDisplayText, ref string __state)
		{
			__state = null;

			if (CommandHandler.CurrentInterface == null)
			{
				modifiedDisplayText = TextUtil.SetEndPadding(modifiedDisplayText.TrimStart('\n', ' '), '\n', 2);
				return true;
			}

			var runVanilla = ProcessInterface(CommandHandler.CurrentInterface, ref modifiedDisplayText, __instance);

			if (!runVanilla)
			{
				__state = modifiedDisplayText;
			}

			return runVanilla;
		}

		[HarmonyPostfix]
		public static string Postfix(string __result, Terminal __instance, string __state)
		{
			// if __state is not null, the vanilla method didn't run.
			string result = __state ?? __result;

			if (CommandHandler.CurrentInterface != null)
			{
				result = CommandHandler.CurrentInterface.PostProcessText(__instance, result);
			}

			return result;
		}

		private static bool ProcessInterface(ITerminalInterface terminalInterface, ref string modifiedDisplayText, Terminal terminal)
		{
			if (terminalInterface.APITextPostProcessing)
			{
				modifiedDisplayText = TextUtil.SetEndPadding(modifiedDisplayText.TrimStart('\n', ' '), '\n', 2);
			}

			modifiedDisplayText = terminalInterface.PreProcessText(terminal, modifiedDisplayText);

			return terminalInterface.VanillaTextPostProcessing;
		}
	}
}
