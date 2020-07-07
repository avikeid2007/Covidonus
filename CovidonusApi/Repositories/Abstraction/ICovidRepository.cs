using CovidonusApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories.Abstraction
{
    public interface ICovidRepository
    {
        Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false);
    }
}