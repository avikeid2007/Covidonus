using CovidonusApi.Models.DTOs;
using System.Collections.Generic;

namespace CovidonusApi.Repositories.Abstraction
{
    public interface ICovidRepository
    {
        IEnumerable<StateData> GetStates();
    }
}