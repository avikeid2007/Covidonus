using CovidonusApi.Models.DTOs;
using CovidonusApi.Repositories.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories
{
    public class CovidRepository : CoreRepository, ICovidRepository
    {
        public async Task<IEnumerable<StateData>> GetStatesAsync()
        {
            if (CoreRepository.MenuList == null || CoreRepository.MenuList?.Count() <= 0)
            {
                await SetUpdatedMenuAsync();
            }
            return CoreRepository.MenuList;

        }
        public IEnumerable<DailyTotalCount> GetDailyTotals()
        {
            if (CoreRepository.DailyTotalCounts == null || CoreRepository.DailyTotalCounts?.Count() <= 0)
            {
                SetDailyCount();
            }
            return CoreRepository.DailyTotalCounts;

        }
    }
}