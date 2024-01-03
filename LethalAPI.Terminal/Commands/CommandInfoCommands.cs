using System;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using GameNetcodeStuff;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Helpers;
using LethalAPI.LibTerminal.Models;
using UnityEngine;

namespace LethalAPI.LibTerminal.Commands
{
    /// <summary>
    /// Contains Terminal Command definitions for the built-in help commands
    /// </summary>
    public class CommandInfoCommands
    {
        [TerminalCommand("Other", clearText: true)]
        public string CommandList()
        {
            var builder = new StringBuilder();

            // Hardcoded commands
            builder.AppendLine(Buffer("Other commands:"));
            builder.AppendLine();
            builder.AppendLine("> VIEW MONITOR");
            builder.AppendLine("To toggle on/off the main monitor's map cam");
            builder.AppendLine();
            builder.AppendLine("> SWITCH {RADAR}");
            builder.AppendLine("To switch the player view on the main monitor");
            builder.AppendLine();
            builder.AppendLine("> PING [Radar booster name]");
            builder.AppendLine("Ping the designated radar booster");
            builder.AppendLine();
            builder.AppendLine("> SCAN");
            builder.AppendLine("To scan for the number of items left on the current planet");
            builder.AppendLine();
            builder.AppendLine("> TRANSMIT [message]");
            builder.AppendLine("Transmit a message with the signal translator");
            builder.AppendLine();

            // Dynamically discovered commands
            // Get all methods from the current type
            var methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods)
            {
                // Check if the method is decorated with the TerminalCommand attribute
                var commandAttr = method.GetCustomAttribute<TerminalCommandAttribute>();
                if (commandAttr != null)
                {
                    // Skip the CommandList method itself
                    if (method.Name.Equals(nameof(CommandList), StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if(!commandAttr.ShowHelp)
                    {
                        continue;
                    }

                    var commandName = commandAttr.CommandName ?? method.Name;
                    var commandInfo = method.GetCustomAttribute<CommandInfoAttribute>();

                    builder.AppendLine($"> {commandName.ToUpper()}");
                    if (commandInfo != null)
                    {
                        if (!commandInfo.Syntax.IsNullOrWhiteSpace())
                            builder.AppendLine($"{commandInfo.Description} - {commandInfo.Syntax}");
                        else
                            builder.AppendLine(commandInfo.Description);
                    }
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }


        [TerminalCommand("Help", clearText: false), CommandInfo("Shows further information about a command", "[Command]")]
        public string HelpCommand(string name)
        {
            var builder = new StringBuilder();
            var commands = TerminalRegistry.EnumerateCommands(name).ToArray();

            if (commands.Length == 0)
            {
                return $"Unknown command: '{name}'";
            }

            foreach (var command in commands)
            {
                builder.AppendLine($">{command.Name.ToUpper()} {command.Syntax?.ToUpper()}");
                builder.AppendLine(command.Description);
                if (!command.CheckAllowed())
                {
                    builder.AppendLine("[Host Only]");
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        [TerminalCommand("Teleport", clearText: false), CommandInfo("Teleport the specified member of the crew", "[Player Name]")]
        //[TeleporterUnlocked]
        public string TeleportCommand(PlayerControllerB player)
        {
            if (player == null)
            {
                return Buffer("Crew member not found!");
            }

            Console.WriteLine("Teleporting");

            var teleporter = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>().Where(t => !t.isInverseTeleporter).FirstOrDefault();

            var canUse = CanUseTeleporter(teleporter);
            if (canUse != null)
                return Buffer(canUse);

            var temp = StartOfRound.Instance.mapScreen.targetedPlayer;

            StartOfRound.Instance.mapScreen.targetedPlayer = player;

            Teleport(teleporter);

            StartOfRound.Instance.mapScreen.targetedPlayer = temp;

            return Buffer($"Teleporting {player.playerUsername}...");
        }

        [TerminalCommand("Teleport", clearText: false), CommandInfo("Teleport the selected member of the crew")]
        //[TeleporterUnlocked]
        public string TeleportCommand()
        {
            Console.WriteLine("Teleporting");

            var teleporter = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>().Where(t => !t.isInverseTeleporter).FirstOrDefault();

            var canUse = CanUseTeleporter(teleporter);
            if (canUse != null)
                return Buffer(canUse);

            var player = StartOfRound.Instance.mapScreen.targetedPlayer;

            Teleport(teleporter);

            return Buffer($"Teleporting {player.playerUsername}...");
        }

        [TerminalCommand("Inverse", clearText: false), CommandInfo("Start the inverse teleporter")]
        //[InverseTeleporterUnlocked]
        public string InverseTeleportCommand()
        {
            Console.WriteLine("Inverse Teleporting");

            var inverse = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>().Where(t => t.isInverseTeleporter).FirstOrDefault();

            var canUse = CanUseTeleporter(inverse);
            if (canUse != null)
                return Buffer(canUse);

            Teleport(inverse);

            return Buffer("Have a safe trip!");
        }

        [TerminalCommand("GetMines", clearText: true), CommandInfo("Lists all landmines")]
        public string GetMines()
        {
            var sb = new StringBuilder();
            var terminalAccessibleObjects = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
            foreach (var tao in terminalAccessibleObjects)
            {
                if (tao.name == "Landmine")
                {
                    sb.AppendLine(tao.objectCode);
                }
            }
            return Buffer(sb.ToString());
        }

        [TerminalCommand("GetTurrets", clearText: true), CommandInfo("Lists all turrets")]
        public string GetTurrets()
        {
            var sb = new StringBuilder();
            var terminalAccessibleObjects = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
            foreach (var tao in terminalAccessibleObjects)
            {
                if (tao.name == "TurretScript")
                {
                    sb.AppendLine(tao.objectCode);
                }
            }
            return Buffer(sb.ToString());
        }

        [TerminalCommand("Detonate", clearText: true), CommandInfo("Detonates designated landmine(s)", "[All / Mine Id]")]
        public string DetonateLandmineCommand(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Buffer("No valid identifier provided!");
            }

            var terminalAccessibleObjects = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
            var landmines = UnityEngine.Object.FindObjectsOfType<Landmine>();
            var selectedObject = terminalAccessibleObjects.Where(tao => tao.objectCode.ToLower() == code.ToLower()).FirstOrDefault();
            if (selectedObject != null)
            {
                var landmine = landmines.Where(l => l.NetworkObjectId == selectedObject.NetworkObjectId).FirstOrDefault();

                if (landmine.hasExploded)
                {
                    return Buffer($"{code} has already detonated!");
                }

                landmine.ExplodeMineServerRpc();
                landmine.Detonate();

                return Buffer($"Kaboom! {code} has been detonated!");
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
                    return Buffer($"Kaboom! Detonating {count} landmines!");
                }
                else
                {
                    return Buffer("There were no mines to detonate!");
                }
            }
            return Buffer($"Landmine {code} not found!");
        }

        //public void SetItemInElevator(bool droppedInShipRoom, bool droppedInElevator, GrabbableObject gObject)
        //{
        //    gObject.isInElevator = droppedInElevator;
        //    if (gObject.isInShipRoom == droppedInShipRoom)
        //    {
        //        return;
        //    }
        //    gObject.isInShipRoom = droppedInShipRoom;
        //    if (!gObject.scrapPersistedThroughRounds)
        //    {
        //        if (droppedInShipRoom)
        //        {
        //            RoundManager.Instance.scrapCollectedInLevel += gObject.scrapValue;
        //            StartOfRound.Instance.gameStats.allPlayerStats[playerClientId].profitable += gObject.scrapValue;
        //            RoundManager.Instance.CollectNewScrapForThisRound(gObject);
        //            gObject.OnBroughtToShip();
        //            if (gObject.itemProperties.isScrap && Vector3.Distance(GameNetworkManager.Instance.localPlayerController.transform.position, gObject.transform.position) < 12f)
        //            {
        //                HUDManager.Instance.DisplayTip("Got scrap!", "To sell, use the terminal to route the ship to the company building.", isWarning: false, useSave: true, "LCTip_SellScrap");
        //            }
        //        }
        //        else
        //        {
        //            if (gObject.scrapPersistedThroughRounds)
        //            {
        //                return;
        //            }
        //            RoundManager.Instance.scrapCollectedInLevel -= gObject.scrapValue;
        //            StartOfRound.Instance.gameStats.allPlayerStats[playerClientId].profitable -= gObject.scrapValue;
        //        }
        //        HUDManager.Instance.SetQuota(RoundManager.Instance.scrapCollectedInLevel);
        //    }
        //    if (droppedInShipRoom)
        //    {
        //        StartOfRound.Instance.currentShipItemCount++;
        //    }
        //    else
        //    {
        //        StartOfRound.Instance.currentShipItemCount--;
        //    }
        //}

        //[TerminalCommand("Personalize", clearText: false), CommandInfo("Colors players on the map")]
        //public string PersonalizeCommand()
        //{
        //    var players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>();

        //    //StartOfRound.Instance.mapScreen.

        //    StartOfRound.Instance.mapScreen.radarTargets.ForEach(target =>
        //    {
        //        target.
        //    });

        //    foreach (var player in players)
        //    {
        //        player.mapRadarDotAnimator.
        //        player.map
        //    }
        //    return null;
        //}

        //[TerminalCommand("Time", clearText: true), CommandInfo("Tells time")]
        public string TimeCommand()
        {
            var time = new TimeHelper().GetTime();

            if(string.IsNullOrEmpty(time))
            {
                return Buffer("There are no clocks in interstellar space.");
            }

            return Buffer($"Time: {time}");
        }

        //[TerminalCommand("Clock", clearText: true), CommandInfo("Tells time")]
        public string ClockCommand()
        {
            //Console.WriteLine("Detonating:", landmine.GetInstanceID());
            //var time = TimeOfDay.Instance.CalculatePlanetTime(TimeOfDay.Instance.currentLevel);
            var time = TimeOfDay.Instance;
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine($"currentLevel: {time.currentLevel}");
            //sb.AppendLine($"globalTimeSpeedMultiplier: {time.globalTimeSpeedMultiplier}");
            //sb.AppendLine($"currentDayTime: {time.currentDayTime}");
            //sb.AppendLine($"hour: {time.hour}");
            //sb.AppendLine($"normalizedTimeOfDay: {time.normalizedTimeOfDay}");
            //sb.AppendLine($"globalTime: {time.globalTime}");
            //sb.AppendLine($"globalTimeAtEndOfDay: {time.globalTimeAtEndOfDay}");
            //sb.AppendLine($"movingGlobalTimeForward: {time.movingGlobalTimeForward}");
            //sb.AppendLine($"quotaVariables: {time.quotaVariables}");
            //sb.AppendLine($"profitQuota: {time.profitQuota}");
            //sb.AppendLine($"quotaFulfilled: {time.quotaFulfilled}");
            //sb.AppendLine($"timesFulfilledQuota: {time.timesFulfilledQuota}");
            //sb.AppendLine($"timeUntilDeadline: {time.timeUntilDeadline}");
            //sb.AppendLine($"daysUntilDeadline: {time.daysUntilDeadline}");
            //sb.AppendLine($"hoursUntilDeadline: {time.hoursUntilDeadline}");
            //sb.AppendLine($"lengthOfHours: {time.lengthOfHours}");
            //sb.AppendLine($"numberOfHours: {time.numberOfHours}");
            //sb.AppendLine($"totalTime: {time.totalTime}");
            //sb.AppendLine($"startingGlobalTime: {TimeOfDay.startingGlobalTime}");
            //sb.AppendLine($"shipLeaveAutomaticallyTime: {time.shipLeaveAutomaticallyTime}");
            //sb.AppendLine($"currentDayTimeStarted: {time.currentDayTimeStarted}");
            //sb.AppendLine($"playersManager: {time.playersManager}");
            //sb.AppendLine($"sunAnimator: {time.sunAnimator}");
            //sb.AppendLine($"sunIndirect: {time.sunIndirect}");
            //sb.AppendLine($"sunDirect: {time.sunDirect}");
            //sb.AppendLine($"insideLighting: {time.insideLighting}");
            //sb.AppendLine($"dayMode: {time.dayMode}");
            //sb.AppendLine($"timeOfDayCues: {time.timeOfDayCues}");
            //sb.AppendLine($"TimeOfDayMusic: {time.TimeOfDayMusic}");
            //sb.AppendLine($"effects: {time.effects}");
            //sb.AppendLine($"currentLevelWeather: {time.currentLevelWeather}");
            //sb.AppendLine($"currentWeatherVariable: {time.currentWeatherVariable}");
            //sb.AppendLine($"currentWeatherVariable2: {time.currentWeatherVariable2}");
            //sb.AppendLine($"currentCompanyMood: {time.currentCompanyMood}");
            ////sb.AppendLine($"CommonCompanyMoods: {time.CommonCompanyMoods}");
            ////sb.AppendLine($"shipLeavingAlertCalled: {time.shipLeavingAlertCalled}");
            //sb.AppendLine($"shipLeavingSoonDialogue: {time.shipLeavingSoonDialogue}");
            //sb.AppendLine($"shipLeavingEarlyDialogue: {time.shipLeavingEarlyDialogue}");
            //sb.AppendLine($"votesForShipToLeaveEarly: {time.votesForShipToLeaveEarly}");
            //sb.AppendLine($"votedShipToLeaveEarlyThisRound: {time.votedShipToLeaveEarlyThisRound}");
            //sb.AppendLine($"onTimeSync: {time.onTimeSync}");
            sb.AppendLine($"globalTimeSpeedMultiplier: {time.globalTimeSpeedMultiplier}");
            sb.AppendLine($"currentDayTime: {time.currentDayTime}");
            sb.AppendLine($"hour: {time.hour}");
            sb.AppendLine($"normalizedTimeOfDay: {time.normalizedTimeOfDay}");
            sb.AppendLine($"globalTime: {time.globalTime}");
            sb.AppendLine($"globalTimeAtEndOfDay: {time.globalTimeAtEndOfDay}");
            sb.AppendLine($"movingGlobalTimeForward: {time.movingGlobalTimeForward}");
            sb.AppendLine($"profitQuota: {time.profitQuota}");
            sb.AppendLine($"quotaFulfilled: {time.quotaFulfilled}");
            sb.AppendLine($"timesFulfilledQuota: {time.timesFulfilledQuota}");
            sb.AppendLine($"timeUntilDeadline: {time.timeUntilDeadline}");
            sb.AppendLine($"daysUntilDeadline: {time.daysUntilDeadline}");
            sb.AppendLine($"hoursUntilDeadline: {time.hoursUntilDeadline}");
            sb.AppendLine($"lengthOfHours: {time.lengthOfHours}");
            sb.AppendLine($"numberOfHours: {time.numberOfHours}");
            sb.AppendLine($"totalTime: {time.totalTime}");
            sb.AppendLine($"startingGlobalTime: {TimeOfDay.startingGlobalTime}");
            sb.AppendLine($"shipLeaveAutomaticallyTime: {time.shipLeaveAutomaticallyTime}");
            sb.AppendLine($"insideLighting: {time.insideLighting}");
            sb.AppendLine($"currentWeatherVariable: {time.currentWeatherVariable}");
            sb.AppendLine($"currentWeatherVariable2: {time.currentWeatherVariable2}");
            sb.AppendLine($"votesForShipToLeaveEarly: {time.votesForShipToLeaveEarly}");
            sb.AppendLine($"votedShipToLeaveEarlyThisRound: {time.votedShipToLeaveEarlyThisRound}");

            return Buffer($"The time is: {sb.ToString()}");
        }

        [TerminalCommand("Berserk", clearText: true), CommandInfo("Makes designated turret(s) go berserk", "[All / Turret Id]")]
        public string TurretBerserkCommand(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Buffer("No valid identifier provided!");
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
                    return Buffer($"{code} has calmed down.");
                }

                turret.EnterBerserkModeServerRpc(player);

                return Buffer($"Danger! {code} has gone berserk!");
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
                    return Buffer($"Danger! {count} turrets have gone berserk!");
                }
                else
                {
                    return Buffer("There were no turrets found!");
                }
            }
            return Buffer($"Turret {code} not found!");
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

        [TerminalCommand("GTFO", clearText: false), CommandInfo("GET ME OUTTA HERE!")]
        public string GTFOCommand()
        {
            if (!StartOfRound.Instance.shipHasLanded)
                return Buffer("You need to land first!");

            Console.WriteLine("GTFO-ing");
            InteractTrigger trigger = GameObject.Find("StartGameLever").GetComponentInChildren<InteractTrigger>();
            var lever = UnityEngine.Object.FindObjectOfType<StartMatchLever>();
            trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            lever.EndGame();
            lever.PlayLeverPullEffectsServerRpc(false);

            return Buffer("RUN AWAY!!!");
        }

        [TerminalCommand("Land", clearText: false), CommandInfo("Land the ship")]
        public string LandCommand()
        {
            if (StartOfRound.Instance.shipHasLanded)
                return Buffer("You're already on the ground, dummy!");

            Console.WriteLine("Landing");
            InteractTrigger trigger = GameObject.Find("StartGameLever").GetComponentInChildren<InteractTrigger>();
            var lever = UnityEngine.Object.FindObjectOfType<StartMatchLever>();
            trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            lever.StartGame();
            lever.PlayLeverPullEffectsServerRpc(true);

            return Buffer("Landing the ship...");
        }

        [TerminalCommand("Inventory", clearText: false), CommandInfo("Get what's out there")]
        public string InventoryCommand()
        {
            Console.WriteLine("Inventory");
            var props = GameObject.FindGameObjectsWithTag("PhysicsProp");
            Console.WriteLine($"Props: {props.Length}");
            StringBuilder sb = new StringBuilder();
            foreach (var prop in props)
            {
                sb.AppendLine($"prop: {prop.name}");
            }
            return Buffer(sb.ToString());
        }

        [TerminalCommand("Unlock", clearText: true)]
        public string UnlockCommand()
        {
            Console.WriteLine("Unlocking");

            var unlockables = UnityEngine.Object.FindObjectOfType<UnlockablesList>();
            Console.WriteLine($"Unlockable count: {unlockables.unlockables.Count}");
            Console.WriteLine($"Unlockables: {unlockables.unlockables}");
            var sb = new StringBuilder();
            foreach (var unlockableItem in unlockables.unlockables)
            {
                sb.AppendLine();
                sb.AppendLine($"unlockableName: {unlockableItem.unlockableName}");
                sb.AppendLine($"unlockableType: {unlockableItem.unlockableType}");
                sb.AppendLine($"alwaysInStock: {unlockableItem.alwaysInStock}");
                sb.AppendLine($"IsPlaceable: {unlockableItem.IsPlaceable}");
                sb.AppendLine($"hasBeenMoved: {unlockableItem.hasBeenMoved}");
                sb.AppendLine($"inStorage: {unlockableItem.inStorage}");
                sb.AppendLine($"canBeStored: {unlockableItem.canBeStored}");
                sb.AppendLine($"maxNumber: {unlockableItem.maxNumber}");
                sb.AppendLine($"hasBeenUnlockedByPlayer: {unlockableItem.hasBeenUnlockedByPlayer}");
                sb.AppendLine($"alreadyUnlocked: {unlockableItem.alreadyUnlocked}");
                sb.AppendLine($"spawnPrefab: {unlockableItem.spawnPrefab}");
                sb.AppendLine();
            }
            return Buffer(sb.ToString());
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
            return Buffer(sb.ToString());
        }

        [TerminalCommand("Status", clearText: true), CommandInfo("Get status of selected crewmembers.", "[Player name]")]
        public string StatusCommand(PlayerControllerB selectedPlayer)
        {
            return Buffer(BuildPlayerStatus(selectedPlayer));
        }

        [TerminalCommand("Lights", clearText: false), CommandInfo("Toggle the lights.")]
        public string LightsCommand()
        {
            InteractTrigger trigger = GameObject.Find("LightSwitch").GetComponent<InteractTrigger>();
            trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

            return Buffer("Toggled lights.");
        }

        [TerminalCommand("ResetInverse", clearText: false), CommandInfo("Resets the inverse teleporter cooldown. DO NOT ABUSE.")]
        public string ResetInverseCommand()
        {
            var inverse = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>().Where(t => t.isInverseTeleporter).FirstOrDefault();

            inverse.buttonTrigger.currentCooldownValue = 0;
            inverse.buttonTrigger.interactable = true;

            return Buffer("Inverse Teleporter Cooldown reset.");
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

        [TerminalCommand("Clear", clearText: true), CommandInfo("Clear the console.")]
        public string ClearCommand()
        {
            return Buffer("");
        }

        [TerminalCommand("GiveMoney", clearText: true, showHelp: false), CommandInfo("Makes you a dirty cheater", "[Amount]")]
        //[AllowedCaller(Models.Enums.AllowedCaller.Host)]
        public string GiveMoneyCommand(int amount)
        {
            var terminal = UnityEngine.Object.FindObjectOfType<Terminal>();

            terminal.groupCredits += amount;

            return Buffer($"${amount} was added, cheater!");
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

        private string BuildPlayerStatus(PlayerControllerB player)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"/// {player.playerUsername} ///");

            if (!player.isPlayerDead)
            {
                sb.AppendLine($"Health: {player.health}%");
            }
            if (player.isPlayerDead)
            {
                sb.AppendLine($"Cause of death: {player.causeOfDeath}");
            }

            sb.AppendLine();
            return sb.ToString();
        }

        private string CanUseDoor()
        {
            if (StartOfRound.Instance.shipHasLanded || StartOfRound.Instance.shipIsLeaving)
                return null;

            return "The ship needs to land first!";
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

        private string Buffer(string input)
        {
            var buffer = new StringBuilder();
            buffer.AppendLine();
            buffer.AppendLine();
            buffer.AppendLine();
            buffer.AppendLine(input);
            Console.WriteLine(buffer.ToString());
            return buffer.ToString();
        }
    }
}
