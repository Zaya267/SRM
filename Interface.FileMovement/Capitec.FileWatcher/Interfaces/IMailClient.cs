using Interface.FileMovement.Models;

namespace Interface.FileMovement.Interfaces
{
    public interface IMailClient
    {
        void SendEmail(MailOptions mailOptions);
    }
}
