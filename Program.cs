using Interface.FileMovement.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Serilog;
using System;
using Topshelf;

namespace Interface.FileMovement
{
    static class Program
    {
        static void Main()
        {
            try
            {
                var services = new ServiceCollection();

                var serviceProvider = Bootstrapper.GetServiceProvider(services);

                SerilogConfig.Logger();

                ServiceConfiguration(serviceProvider);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Service Error.");
            }
        }

        private static void ServiceConfiguration(IServiceProvider serviceProvider)
        {
            HostFactory.Run(configurator =>
            {
                var serviceName = Bootstrapper.Configuration["Serilog:Properties:Application"];

                configurator.SetServiceName(serviceName);
                configurator.SetDisplayName(serviceName);
                configurator.SetDescription("Copy and Archive files.");

                configurator.RunAsLocalSystem();

                configurator.Service<Scheduler>(serviceConfigurator =>
                {
                    var jobFactory = serviceProvider.GetRequiredService<IJobFactory>();

                    serviceConfigurator.ConstructUsing(() => new Scheduler(jobFactory));

                    serviceConfigurator.WhenStarted((service, hostControl) =>
                    {
                        service.OnStart();
                        return true;
                    });

                    serviceConfigurator.WhenStopped((service, hostControl) =>
                    {
                        service.OnStop();
                        return true;
                    });

                    serviceConfigurator.WhenPaused((service, hostControl) =>
                    {
                        service.OnPause();
                        return true;
                    });

                    serviceConfigurator.WhenContinued((service, hostControl) =>
                    {
                        service.OnResume();
                        return true;
                    });
                });
            });
        }
    }
}