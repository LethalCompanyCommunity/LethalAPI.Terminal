using System;
using System.Linq;
using System.Reflection;
using System.Text;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Helpers;
using LethalAPI.LibTerminal.Models;

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
            builder.AppendLine(MiscHelper.Buffer("Other commands:"));
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

            // Dynamically discovered commands across all classes in the namespace
            var assembly = Assembly.GetExecutingAssembly(); // Get the current assembly
            var namespaceName = this.GetType().Namespace; // Get the namespace of the current class

            // Get all types in the assembly within the specific namespace
            var types = assembly.GetTypes()
                .Where(t => String.Equals(t.Namespace, namespaceName, StringComparison.Ordinal));

            foreach (var type in types)
            {
                // Get all public instance methods from the type
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                foreach (var method in methods)
                {
                    // Check if the method is decorated with the TerminalCommand attribute
                    var commandAttr = method.GetCustomAttribute<TerminalCommandAttribute>();
                    if (commandAttr != null)
                    {
                        // Skip the CommandList method itself to avoid recursion
                        if (method.DeclaringType == this.GetType() && method.Name.Equals(nameof(CommandList), StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        if (!commandAttr.ShowHelp)
                        {
                            continue;
                        }

                        var commandName = commandAttr.CommandName ?? method.Name;
                        var commandInfo = method.GetCustomAttribute<CommandInfoAttribute>();

                        builder.AppendLine($"> {commandName.ToUpper()}");
                        if (commandInfo != null)
                        {
                            if (!string.IsNullOrWhiteSpace(commandInfo.Syntax))
                                builder.AppendLine($"{commandInfo.Description} - {commandInfo.Syntax}");
                            else
                                builder.AppendLine(commandInfo.Description);
                        }
                        builder.AppendLine();
                    }
                }
            }

            return builder.ToString();
        }

        //    // Dynamically discovered commands
        //    // Get all methods from the current type
        //    var methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (var method in methods)
        //    {
        //        // Check if the method is decorated with the TerminalCommand attribute
        //        var commandAttr = method.GetCustomAttribute<TerminalCommandAttribute>();
        //        if (commandAttr != null)
        //        {
        //            // Skip the CommandList method itself
        //            if (method.Name.Equals(nameof(CommandList), StringComparison.OrdinalIgnoreCase))
        //            {
        //                continue;
        //            }

        //            if(!commandAttr.ShowHelp)
        //            {
        //                continue;
        //            }

        //            var commandName = commandAttr.CommandName ?? method.Name;
        //            var commandInfo = method.GetCustomAttribute<CommandInfoAttribute>();

        //            builder.AppendLine($"> {commandName.ToUpper()}");
        //            if (commandInfo != null)
        //            {
        //                if (!commandInfo.Syntax.IsNullOrWhiteSpace())
        //                    builder.AppendLine($"{commandInfo.Description} - {commandInfo.Syntax}");
        //                else
        //                    builder.AppendLine(commandInfo.Description);
        //            }
        //            builder.AppendLine();
        //        }
        //    }

        //    return builder.ToString();
        //}


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
    }
}
