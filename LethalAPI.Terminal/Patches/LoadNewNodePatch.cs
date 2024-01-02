using HarmonyLib;

namespace LethalAPI.LibTerminal.Patches
{
	/// <summary>
	/// Patch to catch the last loaded node in the terminal
	/// </summary>
	[HarmonyPatch(typeof(Terminal), "LoadNewNode")]
	internal static class LoadNewNodePatch
	{
		/// <summary>
		/// The last loaded terminal node
		/// </summary>
		public static TerminalNode LastLoadedNode { get; private set; }

		[HarmonyPrefix]
		public static void Prefix(TerminalNode node)
		{
			LastLoadedNode = node;
		}
	}
}
