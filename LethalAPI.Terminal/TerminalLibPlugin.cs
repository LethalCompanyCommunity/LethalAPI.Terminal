using BepInEx;
using HarmonyLib;
using LethalAPI.LibTerminal.Commands;
using LethalAPI.LibTerminal.Models;

namespace LethalAPI.LibTerminal
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class TerminalLibPlugin : BaseUnityPlugin
	{
		private Harmony HarmonyInstance = new Harmony(PluginInfo.PLUGIN_GUID);

		private TerminalModRegistry Terminal;

		private void Awake()
		{
			Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loading...");

			Logger.LogInfo($"Installing patches");
			HarmonyInstance.PatchAll(typeof(TerminalLibPlugin).Assembly);

			Logger.LogInfo($"Registering built-in Commands");

			// Create registry for the Terminals API
			Terminal = TerminalRegistry.CreateTerminalRegistry();

			// Register commands, don't care about the instance
			Terminal.RegisterFrom<CommandInfoCommands>();
			Terminal.RegisterFrom<CheatCommands>();
			Terminal.RegisterFrom<EntityCommands>();
			Terminal.RegisterFrom<EquipmentCommands>();
			Terminal.RegisterFrom<InfoCommands>();
			Terminal.RegisterFrom<PlayerCommands>();
			Terminal.RegisterFrom<ShipCommands>();

            DontDestroyOnLoad(this);

			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
