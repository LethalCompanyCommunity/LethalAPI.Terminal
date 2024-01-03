using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Helpers;

namespace LethalAPI.LibTerminal.Commands
{
    internal class CheatCommands
    {
        [TerminalCommand("GiveMoney", clearText: true, showHelp: false), CommandInfo("Makes you a dirty cheater", "[Amount]")]
        //[AllowedCaller(Models.Enums.AllowedCaller.Host)]
        public string GiveMoneyCommand(int amount)
        {
            var terminal = UnityEngine.Object.FindObjectOfType<Terminal>();

            terminal.groupCredits += amount;

            return MiscHelper.Buffer($"${amount} was added, cheater!");
        }
    }
}
