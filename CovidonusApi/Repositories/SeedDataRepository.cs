using CovidonusApi.Helpers;
using CovidonusApi.Models;
using CovidonusApi.Repositories.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories
{
    public class SeedDataRepository : CoreRepository, ISeedDataRepository
    {
        public async Task SeedCovidDataAsync()
        {
            try
            {
                if (!db.StateWiseDatas.Any())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<StateWiseData> covidData;
                            covidData = await GetStateAndDistrictDataAsync();
                            var stateData = covidData.Select(x => new StateWiseData
                            {
                                State = x.State,
                                StateCode = x.StateCode,
                                Active = x.DistrictData.Sum(a => a.Active),
                                Confirmed = x.DistrictData.Sum(a => a.Confirmed),
                                Deaths = x.DistrictData.Sum(a => a.Deceased),
                                Recovered = x.DistrictData.Sum(a => a.Recovered),
                                Created = DateTime.Now,
                                CreatedBy = "CovidJob",
                                DistrictData = x.DistrictData,
                            }).ToList();
                            stateData.ForEach(x => x.DistrictData.ToList().ForEach(d => { AuditorHelper.MapCreated(d, "CovidJob"); d.StateCode = x.StateCode; }));
                            db.StateWiseDatas.AddRange(stateData);
                            await db.SaveChangesAsync();
                            var dailyData = await GetDailyDataAsync();
                            if (!db.CasesTimeSeries.Any())
                            {
                                var casesTimeSeries = dailyData.Cases_Time_Series.Select(ConvertModels<CasesTimeSeries, CasesTimeSeries>);
                                db.CasesTimeSeries.AddRange(casesTimeSeries);
                                await db.SaveChangesAsync();
                            }
                            if (!db.Testeds.Any())
                            {
                                db.Testeds.AddRange(dailyData.Tested);
                                await db.SaveChangesAsync();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                    db.Configuration.AutoDetectChangesEnabled = true;
                }
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dailyData = await GetDailyDataAsync();
                        if (!db.StateWiseDatas.Any(x => x.StateCode == "TT"))
                        {
                            var totalCount = dailyData.Statewise.FirstOrDefault(x => x.StateCode == "TT");
                            if (totalCount != null)
                            {
                                MapCreated(totalCount);
                                db.StateWiseDatas.Add(totalCount);
                                await db.SaveChangesAsync();
                            }
                        }
                        await UpdateStateDataAsync(dailyData);
                        await UpdateDistrictDataAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task UpdateStateDataAsync(DailyCovidUpdate dailyData)
        {
            var dbRecord = db.StateWiseDatas.ToList();
            foreach (var dbitem in dbRecord)
            {
                var updatedRecord = dailyData.Statewise.FirstOrDefault(x => x.StateCode == dbitem.StateCode);
                MapModels(updatedRecord, dbitem);
                MapModified(dbitem, "CovidJob");
            }
            await db.SaveChangesAsync();
        }
        private async Task UpdateDistrictDataAsync()
        {
            var dbRecord = db.DistrictWiseDatas.Include("Delta").ToList();
            var dailyDistrictData = await GetStateAndDistrictDataAsync();
            foreach (var dbitem in dbRecord)
            {
                var updatedStateRecord = dailyDistrictData.FirstOrDefault(x => x.StateCode == dbitem.StateCode);
                var updatedRecord = updatedStateRecord.DistrictData.FirstOrDefault(x => x.District == dbitem.District);
                if (updatedRecord != null && updatedRecord.Confirmed != dbitem.Confirmed)
                {
                    MapModels(updatedRecord, dbitem);
                    MapModified(dbitem, "CovidJob");
                    if (updatedRecord.Delta.Confirmed != dbitem.Confirmed || updatedRecord.Delta.Deceased != dbitem.Deceased || updatedRecord.Delta.Recovered != dbitem.Recovered)
                    {
                        MapModels(updatedRecord.Delta, dbitem.Delta);
                    }
                }
            }
            await db.SaveChangesAsync();
        }

        private static async Task<List<StateWiseData>> GetStateAndDistrictDataAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://api.covid19india.org/v2/state_district_wise.json");
                return JsonConvert.DeserializeObject<List<StateWiseData>>(response);
            }
        }

        private static async Task<DailyCovidUpdate> GetDailyDataAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://api.covid19india.org/data.json");
                return JsonConvert.DeserializeObject<DailyCovidUpdate>(response, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy HH:mm:ss" });
            }
        }
    }
}
