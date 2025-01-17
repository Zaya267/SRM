using Interface.FileMovement.Database;
using Interface.FileMovement.Interfaces;
using Interface.FileMovement.Models;

namespace Interface.FileMovement.Services
{
    public class SettingsConfig : ISettingsConfig
    {
        public MailOptions GetMailOptions(FileMovementSetting movementSetting)
        {
            return new MailOptions
            {
                MailBCC = movementSetting.MailBcc,
                MailCC = movementSetting.Mailcc,
                MailTo = string.IsNullOrEmpty(movementSetting.MailTo) ? Bootstrapper.Configuration["Mail:To"] : movementSetting.MailTo,
                SMTP = Bootstrapper.Configuration["Mail:SMTP"],
                MailFrom = Bootstrapper.Configuration["Mail:From"]
            };
        }
    }
}