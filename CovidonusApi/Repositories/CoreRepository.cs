using AutoMapper;
using CovidonusApi.Helpers;
using CovidonusApi.Models;
using CovidonusApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories
{
    public class CoreRepository
    {
        protected IMapper mapper;
        protected CovidonusContext db = new CovidonusContext();
        protected static IEnumerable<StateData> MenuList;
        protected static IEnumerable<DailyTotalCount> DailyTotalCounts;

        protected IMapper GetMapper()
        {
            return CovidonusMapper.GetMapper();
        }

        public T ConvertModels<T, T2>(T2 t2)
        {
            if (mapper == null)
            {
                mapper = GetMapper();
            }
            return mapper.Map<T>(t2);
        }

        public void MapModels<T, T2>(T source, T2 destination)
        {
            if (mapper == null)
            {
                mapper = GetMapper();
            }
            mapper.Map(source, destination);
        }

        public static void MapCreated<T>(T obj, string userName = null) where T : Auditor
        {
            if (obj != null)
            {
                obj.Created = DateTime.Now;
                obj.CreatedBy = userName;
                obj.IsActive = true;
            }
        }
        protected void SetDailyCount()
        {
            DailyTotalCounts = db.CasesTimeSeries.AsEnumerable().Select(ConvertModels<DailyTotalCount, CasesTimeSeries>).ToList();
        }
        protected async Task SetUpdatedMenuAsync()
        {
            var tempMenuList = db.StateWiseDatas.Include("DistrictData.Delta").Select(ConvertModels<StateData, StateWiseData>).ToList();
            foreach (var item in tempMenuList)
            {
                var date = DateTime.Now.Date.AddDays(-1);
                var stateTodayCount = await db.DailyStateWiseDatas.Where(x => x.StateCode == item.StateCode && x.LastUpdatedtime > date)
                                        .OrderByDescending(x => x.LastUpdatedtime).FirstOrDefaultAsync();
                item.TodayConfirmed = Convert.ToInt32(stateTodayCount?.Confirmed);
                item.TodayDeaths = Convert.ToInt32(stateTodayCount?.Deaths);
                item.TodayRecovered = Convert.ToInt32(stateTodayCount?.Recovered);
                item.TodayTested = Convert.ToInt32(stateTodayCount?.Tested);
                item.TodayUpdatedtime = stateTodayCount?.LastUpdatedtime;
                foreach (var dist in item.DistrictData)
                {
                    var distTodayCount = await db.DailyDistrictWiseDatas.Where(x => x.StateCode == dist.StateCode && x.District.ToUpper() == dist.District.ToUpper() && x.Created > date)
                                        .OrderByDescending(x => x.Created).FirstOrDefaultAsync();
                    dist.TodayConfirmed = Convert.ToInt32(distTodayCount?.Confirmed);
                    dist.TodayDeaths = Convert.ToInt32(distTodayCount?.Deceased);
                    dist.TodayRecovered = Convert.ToInt32(distTodayCount?.Recovered);
                    dist.TodayTested = Convert.ToInt32(distTodayCount?.Tested);
                    dist.TodayUpdatedtime = stateTodayCount?.LastUpdatedtime;
                }
            }
            MenuList = tempMenuList;
        }
        public static void MapModified<T>(T obj, string userName = null) where T : Auditor
        {
            if (obj != null)
            {
                obj.Modified = DateTime.Now;
                obj.ModifiedBy = userName;
            }
        }
    }
}