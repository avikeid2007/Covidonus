using CovidonusApi.Models;
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
            return db.StateWiseDatas.AsNoTracking().Select(ConvertModels<StateData, StateWiseData>).ToList();
        }
    }
}