using LethalAPI.LibTerminal.Attributes;

namespace LethalApi.LibTerminal.Attributes
{
    public class InverseTeleporterUnlockedAttribute : AccessControlAttribute
    {
        public override bool CheckAllowed()
        {
            if (StartOfRound.Instance == null)
                return false;

            return StartOfRound.Instance.SpawnedShipUnlockables.ContainsKey(6);
        }
    }
}