using LethalAPI.LibTerminal.Attributes;

namespace LethalApi.LibTerminal.Attributes
{
    public class TeleporterUnlockedAttribute : AccessControlAttribute
    {
        public override bool CheckAllowed()
        {
            if (StartOfRound.Instance == null)
                return false;

            return StartOfRound.Instance.SpawnedShipUnlockables.ContainsKey(5);
        }
    }
}