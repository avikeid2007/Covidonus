using CovidonusApi.Models;
using CovidonusApi.Repositories.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace CovidonusApi.Controllers
{
    [RoutePrefix("api/Covid")]
    public class CovidController : ApiController
    {
        ICovidRepository _covidRepository;
        public CovidController(ICovidRepository covidRepository)
        {
            _covidRepository = covidRepository;
        }
        [Route(nameof(GetCovidCountsAsync))]
        public async Task<IEnumerable<StateWiseData>> GetCovidCountsAsync()
        {
            return await _covidRepository.GetCovidCountsAsync();
        }
        [Route(nameof(RefreshCovidCountsAsync))]
        public async Task<IEnumerable<StateWiseData>> RefreshCovidCountsAsync()
        {
            return await _covidRepository.RefreshCovidCountsAsync();
        }
    }
}
