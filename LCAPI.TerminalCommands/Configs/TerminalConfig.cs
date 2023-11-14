using System;
using System.Collections.Generic;
using System.Text;
using LCAPI.TerminalCommands.Attributes;
using LCAPI.TerminalCommands.Models;

namespace LCAPI.TerminalCommands.Configs
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
