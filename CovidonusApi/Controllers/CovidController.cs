using CovidonusApi.Models.DTOs;
using CovidonusApi.Repositories.Abstraction;
using System.Collections.Generic;
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
        [Route(nameof(GetDailyTotals))]
        public IEnumerable<DailyTotalCount> GetDailyTotals()
        {
            return _covidRepository.GetDailyTotals();
        }
    }
}
