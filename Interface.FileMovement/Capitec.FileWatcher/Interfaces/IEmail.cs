using Interface.FileMovement.Models;
using System;

namespace Interface.FileMovement.Interfaces
{
    public interface IEmail
    {
        void Error(MailOptions mailOptions, Exception ex);
    }
}