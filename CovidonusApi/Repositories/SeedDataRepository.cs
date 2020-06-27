using CovidonusApi.Helpers;
using CovidonusApi.Models;
using CovidonusApi.Models.DTOs;
using CovidonusApi.Repositories.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories
{
    public class SeedDataRepository : CoreRepository, ISeedDataRepository
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public async Task SeedCovidDataAsync()
        {
            try
            {
                logger.Info("SeedDataRepository:SeedCovidDataAsync=> Start updating covid data");
                if (!db.StateWiseDatas.Any())
                {
                    logger.Info("SeedDataRepository: Start Seeding initial data");
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
                            logger.Info("SeedDataRepository: Start saving state data ");
                            await db.SaveChangesAsync();
                            logger.Info("SeedDataRepository: End saving state data ");
                            var dailyData = await GetDailyDataAsync();
                            if (!db.CasesTimeSeries.Any())
                            {
                                logger.Info("SeedDataRepository: Start saving CasesTimeSeries data ");
                                var casesTimeSeries = dailyData.Cases_Time_Series.Select(ConvertModels<CasesTimeSeries, CasesTimeSeries>);
                                db.CasesTimeSeries.AddRange(casesTimeSeries);
                                await db.SaveChangesAsync();
                                logger.Info("SeedDataRepository: Start saving CasesTimeSeries data ");
                            }
                            if (!db.Testeds.Any())
                            {
                                logger.Info("SeedDataRepository: Start saving Tested data ");
                                db.Testeds.AddRange(dailyData.Tested);
                                await db.SaveChangesAsync();
                                logger.Info("SeedDataRepository: Start saving Tested data ");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, "SeedCovidDataAsync=> Error in inital seeding");
                            transaction.Rollback();
                            throw;
                        }
                    }
                    db.Configuration.AutoDetectChangesEnabled = true;
                }
                logger.Info("SeedDataRepository: Start Refreshing data");
                var dailyNewData = await GetDailyDataAsync();
                await UpdateDailyStateAndDistrictAsync(dailyNewData);
                await UpdateTodayStateAndDistrictAsync();
                await UpdateDailyTimeSeriesAndTestedAsync(dailyNewData);

                logger.Info("SeedDataRepository: Start Fetching Menu Items");
                await SetUpdatedMenuAsync();
                logger.Info("SeedDataRepository: End Fetching Menu Items");
                logger.Info("SeedDataRepository: Start Fetching Daily total count");
                await SetDailyCountAsync();
                logger.Info("SeedDataRepository: End Fetching Daily total count");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "SeedCovidDataAsync=> Error in Fetching covid");
            }
        }

        private async Task UpdateDailyStateAndDistrictAsync(DailyCovidUpdate dailyData)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    logger.Info("SeedDataRepository:UpdateDailyStateAndDistrictAsync=> Start Refreshing/Updating for new Daily State And District data");
                    if (!db.StateWiseDatas.Any(x => x.StateCode == "TT"))
                    {
                        var totalCount = dailyData.Statewise.FirstOrDefault(x => x.StateCode == "TT");
                        if (totalCount != null)
                        {
                            logger.Info("SeedDataRepository:UpdateDailyStateAndDistrictAsync=> Add total record in StateWiseDatas");
                            MapCreated(totalCount);
                            db.StateWiseDatas.Add(totalCount);
                            await db.SaveChangesAsync();
                            logger.Info("SeedDataRepository:UpdateDailyStateAndDistrictAsync=> Done Add total record in StateWiseDatas");
                        }
                    }
                    await UpdateStateDataAsync(dailyData);
                    await UpdateDistrictDataAsync();
                    transaction.Commit();
                    logger.Info("SeedDataRepository:UpdateDailyStateAndDistrictAsync=> End Refreshing/Updating for Daily State And District new data");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "SeedDataRepository:UpdateDailyStateAndDistrictAsync=> Error in Refreshing/Updating for Daily State And District new data");
                    transaction.Rollback();
                }
            }
        }
        private async Task UpdateDailyTimeSeriesAndTestedAsync(DailyCovidUpdate dailyData)
        {
            try
            {
                logger.Info("SeedDataRepository:UpdateDailyTimeSeriesAndTestedAsync=> Start Refreshing/Updating for new TimeSeries And Tested data");
                await UpdateCaseSeriesAsync(dailyData);
                await UpdateTestedAsync(dailyData);
                logger.Info("SeedDataRepository:UpdateDailyTimeSeriesAndTestedAsync=> End Refreshing/Updating for new TimeSeries And Tested data");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "SeedDataRepository:UpdateDailyTimeSeriesAndTestedAsync=>Error in Refreshing/Updating for new TimeSeries And Tested data");
            }
        }


        private async Task UpdateCaseSeriesAsync(DailyCovidUpdate dailyData)
        {
            logger.Info("SeedDataRepository:UpdateCaseSeriesAsync=> Start update CasesTimeSeries");
            var lastCase = await db.CasesTimeSeries.OrderByDescending(x => x.DateFull).FirstOrDefaultAsync();
            if (lastCase != null)
            {
                var newCaseList = dailyData.Cases_Time_Series.Select(ConvertModels<CasesTimeSeries, CasesTimeSeries>).Where(x => x.DateFull > lastCase.DateFull).ToList();
                if (newCaseList?.Count > 0)
                {
                    db.CasesTimeSeries.AddRange(newCaseList);
                    await db.SaveChangesAsync();
                }
            }
            logger.Info("SeedDataRepository:UpdateCaseSeriesAsync=> End update CasesTimeSeries");
        }
        private async Task UpdateTodayStateAndDistrictAsync()
        {
            logger.Info("SeedDataRepository:UpdateCaseSeriesAsync=> Start update CasesTimeSeries");
            var todayData = await GetTodayStateAndDistrictDataAsync();
            if (todayData?.Count > 0)
            {
                db.DailyStateWiseDatas.AddRange(todayData);
                await db.SaveChangesAsync();
            }
            logger.Info("SeedDataRepository:UpdateCaseSeriesAsync=> End update CasesTimeSeries");
        }
        private async Task UpdateTestedAsync(DailyCovidUpdate dailyData)
        {
            logger.Info("SeedDataRepository:UpdateTestedAsync=> Start update Testeds");
            var lastTested = await db.Testeds.OrderByDescending(x => x.UpdateTimeStamp).FirstOrDefaultAsync();
            if (lastTested != null)
            {
                var newTestedList = dailyData.Tested.Where(x => x.UpdateTimeStamp > lastTested.UpdateTimeStamp).ToList();
                if (newTestedList?.Count > 0)
                {
                    db.Testeds.AddRange(newTestedList);
                    await db.SaveChangesAsync();
                }
            }
            logger.Info("SeedDataRepository:UpdateTestedAsync=> End update Testeds");
        }
        private async Task UpdateStateDataAsync(DailyCovidUpdate dailyData)
        {
            logger.Info("SeedDataRepository:UpdateStateDataAsync=> Start update StateWiseDatas");
            var dbRecord = db.StateWiseDatas.ToList();
            foreach (var dbitem in dbRecord)
            {
                var updatedRecord = dailyData.Statewise.FirstOrDefault(x => x.StateCode == dbitem.StateCode);
                MapModels(updatedRecord, dbitem);
                MapModified(dbitem, "CovidJob");
            }
            await db.SaveChangesAsync();
            logger.Info("SeedDataRepository:UpdateStateDataAsync=> End update StateWiseDatas");
        }

        private async Task UpdateDistrictDataAsync()
        {
            logger.Info("SeedDataRepository:UpdateDistrictDataAsync=> Start update DistrictWiseDatas");
            var dbRecord = db.DistrictWiseDatas.Include("Delta").ToList();
            var dailyDistrictData = await GetStateAndDistrictDataAsync();
            int i = 0;
            foreach (var dbitem in dbRecord)
            {
                var updatedStateRecord = dailyDistrictData.FirstOrDefault(x => x.StateCode == dbitem.StateCode);
                var updatedRecord = updatedStateRecord.DistrictData.FirstOrDefault(x => x.District == dbitem.District);
                if (updatedRecord != null && updatedRecord.Confirmed != dbitem.Confirmed)
                {

                    dbitem.Active = updatedRecord.Active;
                    dbitem.Confirmed = updatedRecord.Confirmed;
                    dbitem.Deceased = updatedRecord.Deceased;
                    dbitem.Notes = updatedRecord.Notes;
                    dbitem.Recovered = updatedRecord.Recovered;
                    dbitem.Delta.Recovered = updatedRecord.Delta.Recovered;
                    dbitem.Delta.Confirmed = updatedRecord.Delta.Confirmed;
                    dbitem.Delta.Deceased = updatedRecord.Delta.Deceased;
                    MapModified(dbitem, "CovidJob");
                    i = i + 1;
                }
            }
            logger.Info($"SeedDataRepository:UpdateDistrictDataAsync=> {i} record need to update DistrictWiseDatas");
            await db.SaveChangesAsync();
            logger.Info("SeedDataRepository:UpdateDistrictDataAsync=> End update DistrictWiseDatas");
        }

        private async Task<List<StateWiseData>> GetStateAndDistrictDataAsync()
        {
            logger.Info("SeedDataRepository: GetStateAndDistrictDataAsync()=> Start fetching state Wise data https://api.covid19india.org/v2/state_district_wise.json");
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://api.covid19india.org/v2/state_district_wise.json");
                logger.Info("SeedDataRepository: GetStateAndDistrictDataAsync()=> End fetching state Wise data https://api.covid19india.org/v2/state_district_wise.json");
                return JsonConvert.DeserializeObject<List<StateWiseData>>(response);
            }
        }
        private async Task<List<DailyStateWiseData>> GetTodayStateAndDistrictDataAsync()
        {
            try
            {
                logger.Info("SeedDataRepository: GetTodayStateAndDistrictDataAsync()=> Start fetching Daily Data state and district wise from https://api.covid19india.org/v3/data.json");
                Dictionary<string, Daily> result;
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync("https://api.covid19india.org/v3/data.json");
                    result = JsonConvert.DeserializeObject<Dictionary<string, Daily>>(response);
                }
                logger.Info("SeedDataRepository: GetTodayStateAndDistrictDataAsync()=> Start fetching Daily Data state and district wise from https://api.covid19india.org/v3/data.json");
                if (result?.Count > 0)
                {
                    int i = 0, j = 0;
                    logger.Info("SeedDataRepository: GetTodayStateAndDistrictDataAsync()=> Start Mapping Daily Data state and district wise");
                    List<DailyStateWiseData> dailyStateList = new List<DailyStateWiseData>();
                    foreach (var item in result)
                    {
                        var validData = await db.DailyStateWiseDatas.AnyAsync(x => x.StateCode == item.Key && x.LastUpdatedtime == item.Value.Meta.LastUpdated);
                        if (!validData)
                        {
                            DailyStateWiseData dailyState;
                            var CurrentState = await db.StateWiseDatas.FirstOrDefaultAsync(x => x.StateCode.Trim().ToUpper() == item.Key.Trim().ToUpper());
                            if (CurrentState != null)
                            {
                                dailyState = new DailyStateWiseData()
                                {
                                    State = CurrentState.State,
                                    StateCode = CurrentState.StateCode,
                                    StateNotes = item.Value?.Meta?.Notes,
                                    LastUpdatedtime = item.Value?.Meta?.LastUpdated,
                                    Confirmed = Convert.ToInt32(item.Value?.Total?.Confirmed),
                                    Recovered = Convert.ToInt32(item.Value?.Total?.Recovered),
                                    Deaths = Convert.ToInt32(item.Value?.Total?.Deceased),
                                    Tested = Convert.ToInt32(item.Value?.Total?.Tested),
                                    StateLogo = CurrentState.StateLogo
                                };
                                i += 1;
                                MapCreated(dailyState, "CovidJob");
                                var newDistricts = item.Value?.Districts;
                                if (newDistricts?.Count > 0)
                                {
                                    foreach (var ditem in newDistricts)
                                    {
                                        var CurrentDistrict = await db.DistrictWiseDatas.FirstOrDefaultAsync(x => x.District.Trim().ToUpper() == ditem.Key.Trim().ToUpper());
                                        if (CurrentDistrict != null)
                                        {
                                            var dailyDist = new DailyDistrictWiseData()
                                            {
                                                District = CurrentDistrict.District,
                                                StateCode = CurrentDistrict.StateCode,
                                                Confirmed = Convert.ToInt32(ditem.Value?.Total?.Confirmed),
                                                Recovered = Convert.ToInt32(ditem.Value?.Total?.Recovered),
                                                Deceased = Convert.ToInt32(ditem.Value?.Total?.Deceased),
                                                Tested = Convert.ToInt32(ditem.Value?.Total?.Tested),
                                            };
                                            MapCreated(dailyDist, "CovidJob");
                                            if (dailyState.DailyDistrictData == null)
                                            {
                                                dailyState.DailyDistrictData = new List<DailyDistrictWiseData>();
                                            }
                                            dailyState.DailyDistrictData.Add(dailyDist);
                                            j += 1;
                                        }

                                    }
                                    dailyStateList.Add(dailyState);
                                }
                            }
                        }
                    }
                    logger.Info($"SeedDataRepository: GetTodayStateAndDistrictDataAsync()=> End Mapping Daily Data state {i} and district wise {j}");
                    return dailyStateList;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private static async Task<DailyCovidUpdate> GetDailyDataAsync()
        {
            logger.Info("SeedDataRepository: GetDailyDataAsync()=> Start fetching daily total and grand total https://api.covid19india.org/data.json");
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://api.covid19india.org/data.json");
                logger.Info("SeedDataRepository: GetDailyDataAsync()=> Start fetching daily total and grand total https://api.covid19india.org/data.json");
                return JsonConvert.DeserializeObject<DailyCovidUpdate>(response, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy HH:mm:ss" });
            }
        }
    }
}
