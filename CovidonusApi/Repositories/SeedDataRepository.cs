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
        private async Task<List<Resource>> GetResourceListAsync()
        {
            logger.Info("RefreshCovidDataAsync: GetStateAndDistrictDataAsync()=> Start fetching state Wise data https://api.covid19india.org/resources/resources.json");
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://api.covid19india.org/resources/resources.json");
                logger.Info("SeedDataRepository: GetStateAndDistrictDataAsync()=> End fetching state Wise data https://api.covid19india.org/resources/resources.json");
                return (JsonConvert.DeserializeObject<CovidResource>(response)).Resources;
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

        private static void AddBotInfo(List<StateWiseData> states)
        {
            states.ForEach(x =>
            {
                switch (x.StateCode.ToUpper())
                {
                    case "AN":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.facebook.com/MyGovArunachalPradesh/";
                        x.EPassLink = "https://southandaman.nic.in/whos-who/";
                        break;
                    case "AP":
                        x.WhatsAppBot = "https://wa.me/918297104104?text=Hi";
                        x.FacebookBot = "https://m.me/ArogyaAndhra";
                        x.EPassLink = "https://www.spandana.ap.gov.in/";
                        break;
                    case "AR":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://eservice.arunachal.gov.in/";
                        break;
                    case "AS":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://eservices.assam.gov.in/directApply.do?serviceId=1533";
                        break;
                    case "BR":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://covid19.bihar.gov.in/";
                        break;
                    case "CH":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "http://admser.chd.nic.in/dpc/";
                        break;
                    case "CT":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://raipur.gov.in/cg-covid-19-epass/";
                        break;
                    case "DL":
                        x.WhatsAppBot = "https://wa.me/918800007722?text=Hi";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://epass.jantasamvad.org/epass/relief/english/";
                        break;
                    case "DN":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://epass.dddcovid19.in/";
                        break;
                    case "GA":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://goaonline.gov.in/";
                        break;
                    case "GJ":
                        x.WhatsAppBot = "https://wa.me/917433000104?text=Hi";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://www.digitalgujarat.gov.in/Citizen/CitizenService.aspx";
                        break;
                    case "HP":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "http://covid19epass.hp.gov.in/";
                        break;
                    case "HR":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://edisha.gov.in/eForms/MigrantService";
                        break;
                    case "JH":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://ranchi.nic.in/e-pass/";
                        break;
                    case "JK":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "http://jkmonitoring.nic.in/";
                        break;
                    case "KA":
                        x.WhatsAppBot = "https://wa.me/918750971717?text=Hi";
                        x.FacebookBot = "https://m.me/KarnatakaVarthe.Official";
                        x.EPassLink = "https://sevasindhu.karnataka.gov.in/Sevasindhu/English";
                        break;
                    case "KL":
                        x.WhatsAppBot = "https://wa.me/919072220183?text=Hi";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://covid19jagratha.kerala.nic.in/home/addDomestic";
                        break;
                    case "LA":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://leh.nic.in/epass/";
                        break;
                    case "LD":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://lakshadweep.gov.in/service/epass-covid19-interstate-movement-pass/";
                        break;
                    case "MH":
                        x.WhatsAppBot = "https://wa.me/912026127394?text=Hi";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://covid19.mhpolice.in/";
                        break;
                    case "ML":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://megedistrict.gov.in/";
                        break;
                    case "MN":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://tengbang.in/";
                        break;
                    case "MP":
                        x.WhatsAppBot = "https://wa.me/917834980000?text=Hi";
                        x.FacebookBot = "https://m.me/jansampark.madhyapradesh";
                        x.EPassLink = "https://mapit.gov.in/covid-19/";
                        break;
                    case "MZ":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://mcovid19.mizoram.gov.in/";
                        break;
                    case "NL":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://www.iamstranded.nagaland.gov.in/";
                        break;
                    case "OR":
                        x.WhatsAppBot = "https://wa.me/919337929000?text=Hi";
                        x.FacebookBot = "http://m.me/HFWOdisha";
                        x.EPassLink = "https://covid19.odisha.gov.in/";
                        break;
                    case "PB":
                        x.WhatsAppBot = "https://wa.me/917380173801?text=Hi";
                        x.FacebookBot = "http://m.me/PunjabGovtIndia";
                        x.EPassLink = "http://covidhelp.punjab.gov.in/";
                        break;
                    case "PY":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://epass.py.gov.in/";
                        break;
                    case "RJ":
                        x.WhatsAppBot = "https://wa.me/911412225624?text=Hi";
                        x.FacebookBot = "http://m.me/RajGovOfficial";
                        x.EPassLink = "https://sso.rajasthan.gov.in/signin";
                        break;
                    case "SK":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://eservices.sikkim.gov.in/directApply.do?serviceId=1364";
                        break;
                    case "TG":
                        x.WhatsAppBot = "https://wa.me/919000658658?text=Hi";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://policeportal.tspolice.gov.in/";
                        break;
                    case "TN":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://tnepass.tnega.org/";
                        break;
                    case "TR":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://covid19.tripura.gov.in/";
                        break;
                    case "UP":
                        x.WhatsAppBot = "https://wa.me/919454441036?text=Hi";
                        x.FacebookBot = "https://m.me/InfoDeptUP";
                        x.EPassLink = "http://jansunwai.up.nic.in/";
                        break;
                    case "UT":
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "http://smartcitydehradun.uk.gov.in/e-pass";
                        break;
                    case "WB":
                        x.WhatsAppBot = "https://wa.me/918697245555?text=Hi";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://coronapass.kolkatapolice.org/";
                        break;
                    default:
                        x.WhatsAppBot = "https://wa.me/919013151515";
                        x.FacebookBot = "https://www.messenger.com/t/MyGovIndia";
                        x.EPassLink = "https://www.mygov.in/covid-19";
                        break;
                }
            });
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
                    AddBotInfo(stateData);
                    MenuList = stateData.AsEnumerable();
                    await LoadResourceListAsync();
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

        private async Task LoadResourceListAsync()
        {
            ResourceList = await GetResourceListAsync();
        }
    }
}

