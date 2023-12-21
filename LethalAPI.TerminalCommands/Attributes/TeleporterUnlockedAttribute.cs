using LethalAPI.TerminalCommands.Attributes;

public class TeleporterUnlockedAttribute : AccessControlAttribute
{
    public override bool CheckAllowed()
    {
        if (StartOfRound.Instance == null)
            return false;

        return StartOfRound.Instance.SpawnedShipUnlockables.ContainsKey(5);
    }
}