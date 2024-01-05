using BepInEx;
using HarmonyLib;
using LethalAPI.LibTerminal.Commands;
using LethalAPI.LibTerminal.Models;

namespace LethalAPI.LibTerminal
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	internal class LibTerminalPlugin : BaseUnityPlugin
	{
		private readonly Harmony m_Harmony = new Harmony(PluginInfo.PLUGIN_GUID);

		private TerminalModRegistry? m_Registry;

		private void Awake()
		{
			Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loading...");

			Logger.LogInfo($"Installing patches");
			m_Harmony.PatchAll(typeof(LibTerminalPlugin).Assembly);

			Logger.LogInfo($"Registering built-in Commands");

			// Create registry for the Terminals API
			m_Registry = TerminalRegistry.CreateTerminalRegistry();

			// Register commands, don't care about the instance
			m_Registry.RegisterFrom<CommandInfoCommands>();

			DontDestroyOnLoad(this);

			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
