using HarmonyLib;
using UnityEngine;

namespace CustomTerminalCommands.Patches
{
	/// <summary>
	/// Removes trailing newlines from the start of terminal responses.
	/// </summary>
	/// <remarks>
	/// This is to address newlines that the game adds to the start of all terminal responses, that slowly forces the terminal window down and off-screen
	/// </remarks>
	[HarmonyPatch(typeof(Terminal), "TextPostProcess")]
	public static class TextPostProcessPatch
	{
		[HarmonyPrefix]
		public static void Prefix(ref string modifiedDisplayText)
		{
			modifiedDisplayText = modifiedDisplayText.TrimStart('\n', ' ');

			if (!modifiedDisplayText.EndsWith('\n'))
			{
				modifiedDisplayText += "\n";
			}
		}
	}
}
