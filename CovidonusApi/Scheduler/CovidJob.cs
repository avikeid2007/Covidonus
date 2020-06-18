using CovidonusApi.Models;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidonusApi.Scheduler
{
    public class CovidJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync("https://api.covid19india.org/v2/state_district_wise.json");
                    var covidData = JsonConvert.DeserializeObject<List<StateWiseData>>(response);
                    CovidonusContext db = new CovidonusContext();
                    covidData.ForEach(x => { x.Created = DateTime.Now; x.DistrictData.ToList().ForEach(d => { d.Created = DateTime.Now; }); });
                    db.StateWiseDatas.AddRange(covidData);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
            }
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