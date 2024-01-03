using GameNetcodeStuff;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace LethalAPI.LibTerminal.Commands
{
    internal class EquipmentCommands
    {
        [TerminalCommand("Teleport", clearText: false), CommandInfo("Teleport the specified member of the crew", "[Player Name]")]
        //[TeleporterUnlocked]
        public string TeleportCommand(PlayerControllerB player)
        {
            if (player == null)
            {
                return MiscHelper.Buffer("Crew member not found!");
            }

            Console.WriteLine("Teleporting");

            var teleporter = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>().Where(t => !t.isInverseTeleporter).FirstOrDefault();

            var canUse = CanUseTeleporter(teleporter);
            if (canUse != null)
                return MiscHelper.Buffer(canUse);

            var temp = StartOfRound.Instance.mapScreen.targetedPlayer;

            StartOfRound.Instance.mapScreen.targetedPlayer = player;

            Teleport(teleporter);

            StartOfRound.Instance.mapScreen.targetedPlayer = temp;

            return MiscHelper.Buffer($"Teleporting {player.playerUsername}...");
        }

        [TerminalCommand("Teleport", clearText: false), CommandInfo("Teleport the selected member of the crew")]
        //[TeleporterUnlocked]
        public string TeleportCommand()
        {
            Console.WriteLine("Teleporting");

            var teleporter = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>().Where(t => !t.isInverseTeleporter).FirstOrDefault();

            var canUse = CanUseTeleporter(teleporter);
            if (canUse != null)
                return MiscHelper.Buffer(canUse);

            var player = StartOfRound.Instance.mapScreen.targetedPlayer;

            Teleport(teleporter);

            return MiscHelper.Buffer($"Teleporting {player.playerUsername}...");
        }

        [TerminalCommand("Inverse", clearText: false), CommandInfo("Start the inverse teleporter")]
        //[InverseTeleporterUnlocked]
        public string InverseTeleportCommand()
        {
            Console.WriteLine("Inverse Teleporting");

            var inverse = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>().Where(t => t.isInverseTeleporter).FirstOrDefault();

            var canUse = CanUseTeleporter(inverse);
            if (canUse != null)
                return MiscHelper.Buffer(canUse);

            Teleport(inverse);

            return MiscHelper.Buffer("Have a safe trip!");
        }

        [TerminalCommand("ResetInverse", clearText: false), CommandInfo("Resets the inverse teleporter cooldown. DO NOT ABUSE.")]
        public string ResetInverseCommand()
        {
            var inverse = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>().Where(t => t.isInverseTeleporter).FirstOrDefault();

            inverse.buttonTrigger.currentCooldownValue = 0;
            inverse.buttonTrigger.interactable = true;

            return MiscHelper.Buffer("Inverse Teleporter Cooldown reset.");
        }

        [TerminalCommand("Reset", clearText: false, showHelp: false), CommandInfo("Shortcut for ResetInverse.")]
        public string ResetCommand()
        {
            return ResetInverseCommand();
        }

        [TerminalCommand("Scramble", clearText: false), CommandInfo("Reset then activate inverse teleporter.")]
        public string ScrambleCommand()
        {
            ResetInverseCommand();
            return InverseTeleportCommand();
        }

        private void Teleport(ShipTeleporter teleporter)
        {
            teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            return;
        }

        private string CanUseTeleporter(ShipTeleporter teleporter)
        {
            if (teleporter is null)
                return "There is no teleporter to use.";

            if (!teleporter.buttonTrigger.interactable)
            {
                try
                {
                    // Get the Type object corresponding to ShipTeleporter
                    Type type = typeof(ShipTeleporter);

                    // Retrieve the FieldInfo for the private field 'cooldownTime'
                    FieldInfo fieldInfo = type.GetField("cooldownTime", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (fieldInfo != null)
                    {
                        // Get the value of the private field 'cooldownTime'
                        int cooldownTimeValue = (int)fieldInfo.GetValue(teleporter);

                        Console.WriteLine("Cooldown Time: " + cooldownTimeValue);

                        return $"The teleporter is on cooldown for {cooldownTimeValue}.";
                    }
                    else
                    {
                        Console.WriteLine("Field 'cooldownTime' not found.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Field 'cooldownTime' not found.");
                }

                return $"The teleporter is on cooldown.";
            }

            return null;
        }
    }
}
