using CovidonusApiV2.Models;
using CovidonusApiV2.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidonusApiV2.Repositories.Abstraction
{
    public interface ICovidRepository
    {
        Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false);
        IEnumerable<Resource> GetResource();
        CovidNews GetNews();
        IEnumerable<InfoGraphic> GetInfoGraphics();
    }
}