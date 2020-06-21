using CovidonusApi.Repositories;
using Quartz;
using System.Threading.Tasks;

namespace CovidonusApi.Scheduler
{
    public class CovidJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            SeedDataRepository repo = new SeedDataRepository();
            await repo.SeedCovidDataAsync();
        }
    }
}