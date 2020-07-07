using CovidonusApi.Models;
using CovidonusApi.Repositories.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories
{
    public class CovidRepository : CoreRepository, ICovidRepository
    {
        ISeedDataRepository _seedDataRepository;
        public CovidRepository(ISeedDataRepository seedDataRepository)
        {
            _seedDataRepository = seedDataRepository;
        }
        public async Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false)
        {
            if (MenuList == null || MenuList?.Count() <= 0 || isRefresh)
                await _seedDataRepository.RefreshCovidDataAsync();
            return MenuList;

        }
        public async Task<IEnumerable<StateWiseData>> RefreshCovidCountsAsync()
        {
            await _seedDataRepository.RefreshCovidDataAsync();
            return MenuList;

        }
    }
}