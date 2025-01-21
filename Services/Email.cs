using Interface.FileMovement.Interfaces;
using Interface.FileMovement.Models;
using System;
using System.Text;

namespace Interface.FileMovement.Services
{
    public class Email : IEmail
    {
        private readonly IMailClient mailClient;
        private readonly string serviceName;

        public Email(IMailClient mailClient)
        {
            this.mailClient = mailClient;
            serviceName = Bootstrapper.Configuration["Serilog:Properties:Application"];
        }

        public void Error(MailOptions mailOptions, Exception ex)
        {
            mailOptions.Subject = string.Concat(serviceName, " - Error");

            var body = new StringBuilder();
            body.Append("Good day<br /><br />The below error occured.");
            body.Append(string.Format("<br /><br />Error: {0}", ex.Message));
            body.Append(string.Format("<br /><br />Source: {0}", ex.Source));
            body.Append(string.Format("<br /><br />Regards,<br />{0}", serviceName));
            mailOptions.Body = body.ToString();

            mailClient.SendEmail(mailOptions);
        }
    }
}