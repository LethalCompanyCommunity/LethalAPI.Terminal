using GameNetcodeStuff;
using LethalAPI.TerminalCommands.Attributes;
using LethalAPI.TerminalCommands.Interactions;
using LethalAPI.TerminalCommands.Interfaces;
using LethalAPI.TerminalCommands.Models.Enums;
using UnityEngine;

namespace LethalAPI.TerminalCommands.Commands
{
	// These commands are temporary, and are being used to test the interactions system.
	// This file will be removed before merging onto the main branch
	public class TestCommands
	{
		[AllowedCaller(AllowedCaller.Host)]
		[TerminalCommand("Kill")]
		public string KillPlayerCommand()
		{
			return $"Usage: Kill [Player]";
		}

		[AllowedCaller(AllowedCaller.Host)]
		[TerminalCommand("Kill"), CommandInfo("Kills a player", syntax: "[Player]")]
		public ITerminalInteraction KillPlayerCommand(PlayerControllerB target)
		{
			return new ConfirmInteraction()
				.WithPrompt($"Kill player {target.playerUsername}?")
				.Confirm(HandleKillPlayer)
				.Deny(() => "Kill aborted")
				.WithContext(target);
		}

		// Since PlayerControllerB is registered as context, it will be taken from the context instance, instead of parsed from user input
		// That means the only argument here parsed from player input is response
		private string HandleKillPlayer(PlayerControllerB target)
		{
			target.KillPlayer(Vector3.zero);

			return $"Killed {target.playerUsername}.";
		}

		[AllowedCaller(AllowedCaller.Host)]
		[TerminalCommand("Payday"), CommandInfo("Spawns or removes credits")]
		public ITerminalInteraction PaydayCommand()
		{
			// Example of delegate in-line
			return new TerminalInteraction()
				.WithPrompt("How many credits do you want?")
				.WithHandler((Terminal terminal, int credits) =>
				{
					// Player entered a valid int, Increment their balance
					terminal.groupCredits += credits;
					terminal.SyncGroupCreditsServerRpc(terminal.groupCredits, terminal.numberOfItemsInDropship);

					return $"New balance: {terminal.groupCredits} credits.";
				})
				.WithHandler(() => "Please input a valid integer"); // User did not input a valid integer
		}
	}
}
