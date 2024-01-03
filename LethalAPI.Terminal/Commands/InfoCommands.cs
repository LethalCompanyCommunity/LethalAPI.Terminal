using GameNetcodeStuff;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Helpers;
using System;
using System.Text;
using UnityEngine;

namespace LethalAPI.LibTerminal.Commands
{
    internal class InfoCommands
    {
        [TerminalCommand("Time", clearText: true), CommandInfo("Tells time")]
        public string TimeCommand()
        {
            var time = TimeHelper.GetTime();

            if (string.IsNullOrEmpty(time))
            {
                return MiscHelper.Buffer("There are no clocks in interstellar space.");
            }

            return MiscHelper.Buffer($"Time: {time}");
        }

        [TerminalCommand("Status", clearText: true), CommandInfo("Get status of all crewmembers.")]
        public string StatusCommand()
        {
            var players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>();
            var sb = new StringBuilder();

            foreach (var player in players)
            {
                if (!player.playerUsername.StartsWith("Player #"))
                {
                    sb.AppendLine(BuildPlayerStatus(player));
                }
            }
            return MiscHelper.Buffer(sb.ToString());
        }

        [TerminalCommand("Status", clearText: true), CommandInfo("Get status of selected crewmembers.", "[Player name]")]
        public string StatusCommand(PlayerControllerB selectedPlayer)
        {
            return MiscHelper.Buffer(BuildPlayerStatus(selectedPlayer));
        }

        [TerminalCommand("Clear", clearText: true), CommandInfo("Clear the console.")]
        public string ClearCommand()
        {
            return MiscHelper.Buffer("");
        }

        private string BuildPlayerStatus(PlayerControllerB player)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"/// {player.playerUsername} ///");

            sb.AppendLine(
                !player.isPlayerDead ?
                $"Health: {player.health}%" :
                $"Cause of death: {player.causeOfDeath}"
            );

            sb.AppendLine();
            return sb.ToString();
        }
    }
}
