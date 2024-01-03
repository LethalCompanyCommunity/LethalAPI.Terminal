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
        [TerminalCommand("Teleport", clearText: false, showHelp: false), CommandInfo("Teleport the specified member of the crew", "[Player Name]")]
        public string TeleportCommand()
        {
            return TeleportCommand(null);
        }

        [TerminalCommand("Teleport", clearText: false), CommandInfo("Teleport the specified member of the crew", "[Player Name]")]
        public string TeleportCommand(PlayerControllerB player = null)
        {
            Console.WriteLine("Teleporting");

            var teleporter = FindTeleporter(false);
            var canUse = CanUseTeleporter(teleporter);
            if (canUse != null)
                return MiscHelper.Buffer(canUse);

            // If a specific player was provided but not found, return an error message.
            if (player == null && StartOfRound.Instance.mapScreen.targetedPlayer == null)
            {
                return MiscHelper.Buffer("No player selected or found to teleport.");
            }

            if (player == null)
            {
                player = StartOfRound.Instance.mapScreen.targetedPlayer;
                Teleport(teleporter);
            }
            else
            {
                var temp = StartOfRound.Instance.mapScreen.targetedPlayer;
                StartOfRound.Instance.mapScreen.targetedPlayer = player;
                Teleport(teleporter);
                StartOfRound.Instance.mapScreen.targetedPlayer = temp;
            }

            return MiscHelper.Buffer($"Teleporting {player.playerUsername}...");
        }

        [TerminalCommand("Inverse", clearText: false), CommandInfo("Start the inverse teleporter")]
        public string InverseTeleportCommand()
        {
            Console.WriteLine("Inverse Teleporting");

            var inverseTeleporter = FindTeleporter(true);
            var canUse = CanUseTeleporter(inverseTeleporter);
            if (canUse != null)
                return MiscHelper.Buffer(canUse);

            Teleport(inverseTeleporter);
            return MiscHelper.Buffer("Have a safe trip!");
        }

        [TerminalCommand("ResetInverse", clearText: false), CommandInfo("Resets the inverse teleporter cooldown. DO NOT ABUSE.")]
        public string ResetInverseCommand()
        {
            var inverseTeleporter = FindTeleporter(true);
            ResetCooldown(inverseTeleporter);
            return MiscHelper.Buffer("Inverse Teleporter Cooldown reset.");
        }

        [TerminalCommand("Reset", clearText: false, showHelp: false), CommandInfo("Shortcut for ResetInverse.")]
        public string ResetCommand() => ResetInverseCommand();

        [TerminalCommand("Scramble", clearText: false), CommandInfo("Reset then activate inverse teleporter.")]
        public string ScrambleCommand()
        {
            ResetInverseCommand();
            return InverseTeleportCommand();
        }

        private ShipTeleporter FindTeleporter(bool isInverse)
        {
            return UnityEngine.Object.FindObjectsOfType<ShipTeleporter>()
                   .FirstOrDefault(t => t.isInverseTeleporter == isInverse);
        }

        private void Teleport(ShipTeleporter teleporter)
        {
            teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
        }

        private void ResetCooldown(ShipTeleporter teleporter)
        {
            teleporter.buttonTrigger.currentCooldownValue = 0;
            teleporter.buttonTrigger.interactable = true;
        }

        private string CanUseTeleporter(ShipTeleporter teleporter)
        {
            if (teleporter is null)
                return "There is no teleporter to use.";

            if (!teleporter.buttonTrigger.interactable)
            {
                var cooldownTimeValue = GetPrivateFieldValue<int>(teleporter, "cooldownTime");
                if (cooldownTimeValue != default)
                {
                    Console.WriteLine("Cooldown Time: " + cooldownTimeValue);
                    return $"The teleporter is on cooldown for {cooldownTimeValue}.";
                }
                Console.WriteLine("Field 'cooldownTime' not found.");
                return "The teleporter is on cooldown.";
            }

            return null;
        }

        private T GetPrivateFieldValue<T>(object instance, string fieldName)
        {
            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                return (T)field.GetValue(instance);
            }
            return default;
        }
    }
}
