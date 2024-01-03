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

            return MiscHelper.Buffer($"The time is: {sb.ToString()}");
        }

        [TerminalCommand("Inventory", clearText: false, showHelp: false), CommandInfo("Get what's out there")]
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
            return MiscHelper.Buffer(sb.ToString());
        }

        [TerminalCommand("Unlock", clearText: true, showHelp: false)]
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
            return MiscHelper.Buffer(sb.ToString());
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
