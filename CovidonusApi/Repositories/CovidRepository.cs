using CovidonusApi.Models.DTOs;
using CovidonusApi.Repositories.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace CovidonusApi.Repositories
{
    public class CovidRepository : CoreRepository, ICovidRepository
    {
        public IEnumerable<StateData> GetStates()
        {
            if (CoreRepository.MenuList == null || CoreRepository.MenuList?.Count() <= 0)
            {
                SetUpdatedMenu();
            }
            return CoreRepository.MenuList;

        }
    }
}