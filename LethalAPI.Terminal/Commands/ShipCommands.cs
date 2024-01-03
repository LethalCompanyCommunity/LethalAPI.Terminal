using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Helpers;
using System;
using System.Linq;
using UnityEngine;

namespace LethalAPI.LibTerminal.Commands
{
    internal class ShipCommands
    {
        [TerminalCommand("Lights", clearText: false), CommandInfo("Toggle the lights.")]
        public string LightsCommand()
        {
            InteractTrigger trigger = GameObject.Find("LightSwitch").GetComponent<InteractTrigger>();
            trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

            return MiscHelper.Buffer("Toggled lights.");
        }

        [TerminalCommand("Doors", clearText: false, showHelp: false)]
        public string ToggleDoorsCommand()
        {
            return ToggleDoorCommand();
        }

        [TerminalCommand("Door", clearText: false), CommandInfo("Toggle the door")]
        public string ToggleDoorCommand()
        {
            return StartOfRound.Instance.hangarDoorsClosed ? OpenCommand() : CloseCommand();
        }

        [TerminalCommand("Close", clearText: false), CommandInfo("Close the door")]
        public string CloseCommand()
        {
            Console.WriteLine("Closing Door");
            var canUse = CanUseDoor();
            if (canUse != null) return canUse;

            TriggerInteract("StopButton");
            var door = FindHangarShipDoor();
            door.PlayDoorAnimation(true);
            return $"Closing Door. Door power: {GetDoorPower(door.doorPower)}";
        }

        [TerminalCommand("Open", clearText: false), CommandInfo("Open the door")]
        public string OpenCommand()
        {
            Console.WriteLine("Opening Door");
            var canUse = CanUseDoor();
            if (canUse != null) return canUse;

            TriggerInteract("StartButton");
            var door = FindHangarShipDoor();
            door.PlayDoorAnimation(false);
            return $"Opening Door. Door power: {GetDoorPower(door.doorPower)}";
        }

        [TerminalCommand("Launch", clearText: false), CommandInfo("Launch the ship")]
        public string LaunchCommand()
        {
            if (!StartOfRound.Instance.shipHasLanded)
                return MiscHelper.Buffer("You need to land first!");

            InteractTrigger trigger = GameObject.Find("StartGameLever").GetComponentInChildren<InteractTrigger>();
            var lever = UnityEngine.Object.FindObjectOfType<StartMatchLever>();
            trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            lever.EndGame();
            lever.PlayLeverPullEffectsServerRpc(false);

            return MiscHelper.Buffer("Launching the ship...");
        }

        [TerminalCommand("GTFO", clearText: false, showHelp: false), CommandInfo("GET ME OUTTA HERE!")]
        public string GTFOCommand()
        {
            LaunchCommand();
            return MiscHelper.Buffer("RUN AWAY!!!1!!");
        }

        [TerminalCommand("Land", clearText: false), CommandInfo("Land the ship")]
        public string LandCommand()
        {
            if (StartOfRound.Instance.shipHasLanded)
                return MiscHelper.Buffer("You're already on the ground, dummy!");

            Console.WriteLine("Landing");
            InteractTrigger trigger = GameObject.Find("StartGameLever").GetComponentInChildren<InteractTrigger>();
            var lever = UnityEngine.Object.FindObjectOfType<StartMatchLever>();
            trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            lever.StartGame();
            lever.PlayLeverPullEffectsServerRpc(true);

            return MiscHelper.Buffer("Landing the ship...");
        }

        private string CanUseDoor()
        {
            if (MiscHelper.IsGameStarted())
                return null;

            return "You really shouldn't mess with the doors while in orbit.";
        }

        private string GetDoorPower(float doorPower)
        {
            return string.Format("{0:0\\%}", doorPower * 100);
        }

        private void TriggerInteract(string buttonName)
        {
            InteractTrigger trigger = GameObject.Find(buttonName).GetComponentInChildren<InteractTrigger>();
            trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
        }

        private HangarShipDoor FindHangarShipDoor()
        {
            return UnityEngine.Object.FindObjectsOfType<HangarShipDoor>().FirstOrDefault();
        }
    }
}
