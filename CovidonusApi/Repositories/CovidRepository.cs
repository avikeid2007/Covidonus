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
            if (MenuList == null || MenuList?.Count() <= 0)
            {
                await SetUpdatedMenuAsync();
            }
            return MenuList;

        }
        public async Task<DailyTotalCount> GetDailyTotalsAsync()
        {
            if (DailyTotalCounts == null)
            {
                await SetDailyCountAsync();
            }
            return DailyTotalCounts;

        }

    }
}