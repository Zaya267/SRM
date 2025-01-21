using Interface.FileMovement.Interfaces;
using Interface.FileMovement.Models;
using System;
using System.Net.Mail;

namespace Interface.FileMovement.Services
{
    public class MailClient : IMailClient
    {
        public void SendEmail(MailOptions mailOptions)
        {
            if (!string.IsNullOrEmpty(mailOptions.MailTo))
            {
                using SmtpClient smtpClient = new SmtpClient(mailOptions.SMTP);
                smtpClient.Send(FormatMesage(mailOptions));
            }
        }

        private MailMessage FormatMesage(MailOptions mailOptions)
        {
            MailMessage mailMessage = new MailMessage
            {
                Body = mailOptions.Body,
                Subject = mailOptions.Subject,
                IsBodyHtml = true,
                From = new MailAddress(mailOptions.MailFrom)
            };

            FormatAddresses(mailOptions.MailTo, mailMessage.To);
            FormatAddresses(mailOptions.MailCC, mailMessage.CC);
            FormatAddresses(mailOptions.MailBCC, mailMessage.Bcc);

            return mailMessage;
        }

        private void FormatAddresses(string mailingList, MailAddressCollection mailAddresses)
        {
            if (!string.IsNullOrEmpty(mailingList))
            {
                if (mailingList.Contains(";"))
                {
                    foreach (var address in mailingList.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mailAddresses.Add(address);
                    }
                }
                else
                    mailAddresses.Add(new MailAddress(mailingList));
            }
        }
    }
}