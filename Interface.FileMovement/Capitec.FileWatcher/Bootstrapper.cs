using Interface.FileMovement.Interfaces;
using Interface.FileMovement.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;

namespace Interface.FileMovement
{
    static class Bootstrapper
    {
        private static readonly IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile($"appsettings.json", true, true)
            .Build();

        public static ref readonly IConfiguration Configuration => ref configuration;

        public static IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            services.AddSingleton<IJobFactory>(provider =>
            {
                var jobFactory = new JobFactory(provider);
                return jobFactory;
            });

            services.AddSingleton<IEmail, Email>();
            services.AddSingleton<IMailClient, MailClient>();
            services.AddSingleton<IFileArchive, FileArchive>();
            services.AddSingleton<IDatabase, Services.Database>();
            services.AddSingleton<IFileValidator, FileValidator>();
            services.AddSingleton<ISettingsConfig, SettingsConfig>();

            services.AddSingleton<FileWatcherJob>();

            RegisterConfigurations(services);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        public static void RegisterConfigurations(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }
        }
    }
}