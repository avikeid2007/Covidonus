using Quartz;
using Quartz.Impl;

namespace CovidonusApi.Scheduler
{
    public class JobScheduler
    {
        public static async void StartAsync()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            IJobDetail job = JobBuilder.Create<CovidJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("CovidJob", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(15)
            .RepeatForever())
            .Build();
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}