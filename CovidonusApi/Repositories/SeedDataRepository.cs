using CovidonusApi.Models;
using CovidonusApi.Models.DTOs;
using CovidonusApi.Repositories.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories
{
    public class SeedDataRepository : CoreRepository, ISeedDataRepository
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private async Task<List<StateWiseData>> GetStateAndDistrictDataAsync()
        {
            logger.Info("RefreshCovidDataAsync: GetStateAndDistrictDataAsync()=> Start fetching state Wise data https://api.covid19india.org/v2/state_district_wise.json");
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://api.covid19india.org/v2/state_district_wise.json");
                logger.Info("SeedDataRepository: GetStateAndDistrictDataAsync()=> End fetching state Wise data https://api.covid19india.org/v2/state_district_wise.json");
                return JsonConvert.DeserializeObject<List<StateWiseData>>(response);
            }
        }
        private static async Task<Dictionary<string, Daily>> GetTodayDataAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://api.covid19india.org/v3/data.json");
                return JsonConvert.DeserializeObject<Dictionary<string, Daily>>(response);
            }
        }
        private static async Task<DailyCovidUpdate> GetDailyDataAsync()
        {
            logger.Info("RefreshCovidDataAsync: GetDailyDataAsync()=> Start fetching daily total and grand total https://api.covid19india.org/data.json");
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://api.covid19india.org/data.json");
                logger.Info("SeedDataRepository: GetDailyDataAsync()=> Start fetching daily total and grand total https://api.covid19india.org/data.json");
                return JsonConvert.DeserializeObject<DailyCovidUpdate>(response, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy HH:mm:ss" });
            }
        }
        private static string GetPercentage(int total, int current)
        {
            if (total == 0)
            {
                return string.Empty;
            }
            double per = (double)current / (double)total * 100;
            return Convert.ToString(Math.Round(per, 2)) + "%";
        }
        public async Task RefreshCovidDataAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            logger.Info("RefreshCovidDataAsync: Start refresh StateWiseData" + DateTime.Now);
            try
            {
                var stateData = await GetStateAndDistrictDataAsync();
                var dailyData = await GetDailyDataAsync();
                if (stateData?.Count > 0 && dailyData != null && dailyData.Statewise?.Count > 0)
                {
                    stateData.ForEach(x =>
                    {
                        x.DistrictData.ToList().ForEach(d =>
                        {
                            d.StateCode = x.StateCode;
                            d.RecoverRate = GetPercentage(d.Confirmed, d.Recovered);
                            d.DeathRate = GetPercentage(d.Confirmed, d.Deceased);
                        });
                    });
                    if (!stateData.Any(x => x.StateCode == "TT"))
                    {
                        logger.Info("RefreshCovidDataAsync: Start adding data in StateWiseData");
                        var totalCount = dailyData.Statewise.FirstOrDefault(x => x.StateCode == "TT");
                        if (totalCount != null)
                        {
                            var todayIndia = dailyData.Cases_Time_Series.Select(ConvertModels<CasesTimeSeries, CasesTimeSeries>).OrderByDescending(x => x.DateFull).FirstOrDefault();
                            var todayTestIndia = dailyData.Tested.OrderByDescending(x => x.UpdateTimeStamp).FirstOrDefault();
                            totalCount.State = "India";
                            if (todayIndia != null)
                            {
                                logger.Info("RefreshCovidDataAsync: Start adding India Today data ");
                                totalCount.TodayConfirmed = todayIndia.DailyConfirmed;
                                totalCount.TodayDeaths = todayIndia.DailyDeceased;
                                totalCount.TodayRecovered = todayIndia.TotalRecovered;
                                logger.Info("RefreshCovidDataAsync: End adding India Today data ");
                            }
                            if (todayTestIndia != null)
                            {
                                logger.Info("RefreshCovidDataAsync: Start adding India Test data");
                                totalCount.Tested = Convert.ToInt32(todayTestIndia.TotalSamplesTested);
                                totalCount.TodayTested = Convert.ToInt32(todayTestIndia.SampleReportedToday);
                                totalCount.TestPerMillion = Convert.ToInt32(todayTestIndia.TestsPerMillion);
                                totalCount.TestSource = todayTestIndia.Source;
                                totalCount.TotalPositiveCases = todayTestIndia.TotalPositiveCases;
                                logger.Info("RefreshCovidDataAsync: End adding India Test data");
                            }
                            stateData.Add(totalCount);
                            logger.Info("SeedDataRepository:RefreshCovidDataAsync=> Done Add total record in StateWiseDatas");
                        }
                    }
                    logger.Info("SeedDataRepository:RefreshCovidDataAsync=> Start update StateWiseDatas & district");
                    Dictionary<string, Daily> result = await GetTodayDataAsync();
                    foreach (var dbitem in stateData)
                    {
                        var updatedRecord = dailyData.Statewise.FirstOrDefault(x => x.StateCode == dbitem.StateCode);
                        MapModels(updatedRecord, dbitem);
                        dbitem.RecoverRate = GetPercentage(dbitem.Confirmed, dbitem.Recovered);
                        dbitem.DeathRate = GetPercentage(dbitem.Confirmed, dbitem.Deaths);
                        dbitem.StateLogo = $"http://covidonusapi.avnishkumar.co.in/Images/{dbitem.StateCode}.png";
                        if (result?.ContainsKey(dbitem.StateCode) == true && dbitem.StateCode != "TT")
                        {
                            dbitem.Population = result[dbitem.StateCode]?.Meta?.Population;
                            dbitem.Tested = Convert.ToInt32(result[dbitem.StateCode].Total?.Tested);
                            if (result[dbitem.StateCode].Delta != null)
                            {
                                dbitem.TodayConfirmed = result[dbitem.StateCode].Delta.Confirmed;
                                dbitem.TodayDeaths = result[dbitem.StateCode].Delta.Deceased;
                                dbitem.TodayRecovered = result[dbitem.StateCode].Delta.Recovered;
                                dbitem.TodayTested = result[dbitem.StateCode].Delta.Tested;
                                if (result[dbitem.StateCode].Districts?.Count > 0)
                                {
                                    foreach (var dist in dbitem.DistrictData)
                                    {
                                        if (result[dbitem.StateCode].Districts.ContainsKey(dist.District))
                                        {
                                            if (result[dbitem.StateCode].Districts[dist.District].Delta != null)
                                            {

                                                dist.TodayConfirmed = result[dbitem.StateCode].Districts[dist.District].Delta.Confirmed;
                                                dist.TodayDeaths = result[dbitem.StateCode].Districts[dist.District].Delta.Deceased;
                                                dist.TodayRecovered = result[dbitem.StateCode].Districts[dist.District].Delta.Recovered;
                                                dist.TodayTested = result[dbitem.StateCode].Districts[dist.District].Delta.Tested;
                                            }
                                            if (result[dbitem.StateCode].Districts[dist.District].Meta != null)
                                            {
                                                dist.Population = result[dbitem.StateCode].Districts[dist.District].Meta.Population;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    MenuList = stateData.AsEnumerable();
                    logger.Info("SeedDataRepository:RefreshCovidDataAsync=> End update StateWiseDatas & district");
                }
            }
            catch (Exception ex)
            {
                logger.Info("SeedDataRepository:RefreshCovidDataAsync=> Error occur." + ex);
            }
            stopwatch.Stop();
            logger.Info("SeedDataRepository:RefreshCovidDataAsync=> Complete refresh in " + stopwatch.Elapsed);
        }
    }
}

