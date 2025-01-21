using Serilog;
using Serilog.Sinks.Email;
using System;

namespace Interface.FileMovement
{
    public static class SerilogConfig
    {
        public static void Logger()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Bootstrapper.Configuration)
                .Enrich.WithMachineName()
                .WriteTo.Email(connectionInfo: new EmailConnectionInfo
                {
                    FromEmail = Bootstrapper.Configuration["Mail:From"],
                    ToEmail = Bootstrapper.Configuration["Mail:To"],
                    MailServer = Bootstrapper.Configuration["Mail:SMTP"],
                    EmailSubject = string.Concat(Bootstrapper.Configuration["Serilog:Properties:Application"], " - Log"),
                    EnableSsl = false,
                    ServerCertificateValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    }
                },
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            Log.Information(string.Concat("Service Started: ", Environment.MachineName));
        }
    }
}