using CovidonusApi.Models;
using CovidonusApi.Models.DTOs;
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
        public async Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false)
        {
            return await _covidRepository.GetCovidCountsAsync(isRefresh);
        }
        [Route(nameof(GetResource))]
        public IEnumerable<Resource> GetResource()
        {
            return _covidRepository.GetResource();
        }
        [Route(nameof(GetNews))]
        public CovidNews GetNews()
        {
            return _covidRepository.GetNews();
        }
        [Route(nameof(GetInfoGraphics))]
        public IEnumerable<InfoGraphic> GetInfoGraphics()
        {
            return _covidRepository.GetInfoGraphics();
        }
    }
}
