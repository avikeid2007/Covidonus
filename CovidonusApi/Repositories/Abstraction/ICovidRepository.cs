using CovidonusApi.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories.Abstraction
{
    public interface ICovidRepository
    {
        Task<IEnumerable<StateData>> GetStatesAsync();
        Task<DailyTotalCount> GetDailyTotalsAsync();

    }
}