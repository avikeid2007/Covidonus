using CovidonusApi.Models;
using CovidonusApi.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories.Abstraction
{
    public interface ICovidRepository
    {
        Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false);
        IEnumerable<Resource> GetResource();
        CovidNews GetNews();
    }
}