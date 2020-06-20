using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace CovidonusApi.Scheduler
{
    public class CovidJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {

        }
    }
    public class JobScheduler
    {
        public static async void StartAsync()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<CovidJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInMinutes(1)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}