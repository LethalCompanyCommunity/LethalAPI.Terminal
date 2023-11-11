using BepInEx;
using HarmonyLib;
using LCAPI.TerminalCommands.Commands;
using LCAPI.TerminalCommands.Models;

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

			DontDestroyOnLoad(this);

			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
