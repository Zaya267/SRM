using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Serilog;
using System;

namespace Interface.FileMovement.Services
{
    public class Scheduler
    {
        private readonly IJobFactory jobFactory;
        private readonly string serverName;

        public Scheduler(IJobFactory jobFactory)
        {
            this.jobFactory = jobFactory;
            serverName = Environment.MachineName;
        }

        public void OnStart()
        {
            try
            {
                var schedulerFactory = new StdSchedulerFactory();

                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = jobFactory;
                scheduler.Start().Wait();

                var fileWatcherJob = JobBuilder.Create<FileWatcherJob>().WithIdentity(JobKey.Create("FileWatcherJob")).Build();

                var scheduleMinutes = int.Parse(Bootstrapper.Configuration["ScheduleMinutes"].ToString());

                var fileWatcherTrigger = TriggerBuilder.Create()
                    .WithIdentity(new TriggerKey("FileWatcher Trigger"))
                    .StartNow()
                    .WithSimpleSchedule(builder => { builder.WithIntervalInMinutes(scheduleMinutes).RepeatForever(); })
                    .ForJob("FileWatcherJob")
                    .Build();

                scheduler.ScheduleJob(fileWatcherJob, fileWatcherTrigger).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Service Error.");
            }
        }

        public void OnStop()
        {
            Log.Information(string.Concat("Service Stopped: ", serverName));
        }

        public void OnPause()
        {
            Log.Information(string.Concat("Service Paused: ", serverName));
        }

        public void OnResume()
        {
            Log.Information(string.Concat("Service Resumed: ", serverName));
        }
    }
}