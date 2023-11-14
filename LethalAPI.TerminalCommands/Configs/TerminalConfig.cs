using System;
using System.Collections.Generic;
using System.Text;
using LethalAPI.TerminalCommands.Attributes;
using LethalAPI.TerminalCommands.Models;

namespace LethalAPI.TerminalCommands.Configs
{
	[ConfigGroup("Terminal", "Configure the behavior of the terminal")]
	public class TerminalConfig
	{
		[TerminalConfig("Enables/Disables terminal verb commands")]
		[ConfigPersist(PersistType.LocalPlayer)]
		public bool VerbsEnabled { get; set; }


		[TerminalConfig("Specifies if the Confirm/Deny pop-up should be shown")]
		[ConfigPersist(PersistType.LocalPlayer)]
		public bool AutoConfirm { get; set; }
	}
}
