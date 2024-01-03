using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Helpers;
using System;
using System.Linq;
using System.Text;
using Unity.Netcode;

namespace LethalAPI.LibTerminal.Commands
{
    internal class EntityCommands
    {
        [TerminalCommand("ListMines", clearText: true), CommandInfo("Lists all landmines")]
        public string ListMines() => ListObjectsByName("Landmine");

        [TerminalCommand("ListTurrets", clearText: true), CommandInfo("Lists all turrets")]
        public string ListTurrets() => ListObjectsByName("TurretScript");

        [TerminalCommand("ListDoors", clearText: true), CommandInfo("Lists all facility doors")]
        public string ListDoors() => ListObjectsByName("BigDoor", true);

        [TerminalCommand("Detonate", clearText: true), CommandInfo("Detonates designated landmine(s)", "[All / Mine Id]")]
        public string DetonateLandmineCommand(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return MiscHelper.Buffer("No valid identifier provided!");
            }

            var terminalAccessibleObjects = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
            var landmines = UnityEngine.Object.FindObjectsOfType<Landmine>();
            var selectedObject = terminalAccessibleObjects.Where(tao => tao.objectCode.ToLower() == code.ToLower()).FirstOrDefault();
            if (selectedObject != null)
            {
                var landmine = landmines.Where(l => l.NetworkObjectId == selectedObject.NetworkObjectId).FirstOrDefault();

                if (landmine.hasExploded)
                {
                    return MiscHelper.Buffer($"{code} has already detonated!");
                }

                landmine.ExplodeMineServerRpc();
                landmine.Detonate();

                return MiscHelper.Buffer($"Kaboom! {code} has been detonated!");
            }
            if (code.ToLower() == "all")
            {
                int count = 0;
                foreach (var landmine in landmines)
                {
                    if (landmine.hasExploded)
                    {
                        continue;
                    }

                    count++;

                    landmine.ExplodeMineServerRpc();
                    landmine.Detonate();
                }
                if (count > 0)
                {
                    return MiscHelper.Buffer($"Kaboom! Detonating {count} landmines!");
                }
                else
                {
                    return MiscHelper.Buffer("There were no mines to detonate!");
                }
            }
            return MiscHelper.Buffer($"Landmine {code} not found!");
        }

        [TerminalCommand("Berserk", clearText: true), CommandInfo("Makes designated turret(s) go berserk", "[All / Turret Id]")]
        public string TurretBerserkCommand(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return MiscHelper.Buffer("No valid identifier provided!");
            }

            var player = ((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
            var terminalAccessibleObjects = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
            var turrets = UnityEngine.Object.FindObjectsOfType<Turret>();
            var selectedObject = terminalAccessibleObjects.Where(tao => tao.objectCode.ToLower() == code.ToLower()).FirstOrDefault();
            if (selectedObject != null)
            {
                var turret = turrets.Where(l => l.NetworkObjectId == selectedObject.NetworkObjectId).FirstOrDefault();
                var isBerserk = turret.turretMode == TurretMode.Berserk;

                if (isBerserk)
                {
                    turret.SetToModeClientRpc((int)TurretMode.Detection);
                    return MiscHelper.Buffer($"{code} has calmed down.");
                }

                turret.EnterBerserkModeServerRpc(player);

                return MiscHelper.Buffer($"Danger! {code} has gone berserk!");
            }
            if (code.ToLower() == "all")
            {
                int count = 0;
                foreach (var turret in turrets)
                {
                    count++;

                    turret.EnterBerserkModeServerRpc(player);
                }
                if (count > 0)
                {
                    return MiscHelper.Buffer($"Danger! {count} turrets have gone berserk!");
                }
                else
                {
                    return MiscHelper.Buffer("There were no turrets found!");
                }
            }
            return MiscHelper.Buffer($"Turret {code} not found!");
        }

        private string ListObjectsByName(string objectName, bool startsWith = false)
        {
            var sb = new StringBuilder();
            var terminalAccessibleObjects = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
            foreach (var tao in terminalAccessibleObjects)
            {
                bool nameMatches = startsWith ? tao.name.StartsWith(objectName) : tao.name == objectName;
                if (nameMatches)
                {
                    sb.AppendLine(tao.objectCode);
                }
            }
            return MiscHelper.Buffer(sb.ToString());
        }
    }
}
