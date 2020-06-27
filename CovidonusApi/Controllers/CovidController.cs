using CovidonusApi.Models.DTOs;
using CovidonusApi.Repositories.Abstraction;
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
        [Route(nameof(GetDailyTotalsAsync))]
        public async Task<DailyTotalCount> GetDailyTotalsAsync()
        {
            return await _covidRepository.GetDailyTotalsAsync();
        }
    }
}
