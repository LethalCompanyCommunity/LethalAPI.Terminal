using LethalAPI.TerminalCommands.Attributes;
using LethalAPI.TerminalCommands.Models.Configuration;

namespace LethalAPI.TerminalCommands.Configs
{
    [ConfigGroup("Terminal", "Configure the behavior of the terminal")]
	public class TerminalConfig
	{
		[TerminalConfig("Enables/Disables terminal verb commands", "VerbsEnabled")]
		[ConfigPersist(PersistType.LocalPlayer)]
		public bool VerbsEnabled { get; set; }
	}
}
