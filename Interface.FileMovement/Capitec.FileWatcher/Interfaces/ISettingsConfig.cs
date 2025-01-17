using Interface.FileMovement.Database;
using Interface.FileMovement.Models;

namespace Interface.FileMovement.Interfaces
{
    public interface ISettingsConfig
    {
        MailOptions GetMailOptions(FileMovementSetting movementSetting);
    }
}
