using LethalAPI.TerminalCommands.Attributes;
//using UnityEngine;
//using System.Linq;

public class InverseTeleporterUnlockedAttribute : AccessControlAttribute
{
    public override bool CheckAllowed()
    {
        if (StartOfRound.Instance == null)
            return false;

        //Object.FindObjectsOfType<ShipTeleporter>().Where(t => t.isInverseTeleporter).First();

        return StartOfRound.Instance.SpawnedShipUnlockables.ContainsKey(6);
    }
}