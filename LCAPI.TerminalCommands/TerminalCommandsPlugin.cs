using BepInEx;
using CustomTerminalCommands.Commands;
using CustomTerminalCommands.Models;
using HarmonyLib;

namespace LCAPI.TerminalCommands
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class TerminalCommandsPlugin : BaseUnityPlugin
	{
		private Harmony HarmonyInstance = new Harmony(PluginInfo.PLUGIN_GUID);

		private ModCommands BuiltInCommands;

		private void Awake()
		{
			Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loading...");
			Logger.LogInfo($"Installing patches");
			HarmonyInstance.PatchAll(typeof(TerminalCommandsPlugin).Assembly);

			Logger.LogInfo($"Installing built-in commands");
			BuiltInCommands = CommandRegistry.CreateModRegistry();
			BuiltInCommands.RegisterFrom<CommandInfoCommands>();

			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}

		private void OnDisable()
		{
			BuiltInCommands?.Deregister();
		}
	}
}
