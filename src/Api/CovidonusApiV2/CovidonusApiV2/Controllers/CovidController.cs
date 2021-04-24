using CovidonusApiV2.Models;
using CovidonusApiV2.Models.DTOs;
using CovidonusApiV2.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidonusApiV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CovidController : ControllerBase
    {

        ICovidRepository _covidRepository;
        public CovidController(ICovidRepository covidRepository)
        {
            _covidRepository = covidRepository;
        }
        [HttpGet("Counts")]
        public async Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false)
        {
            return await _covidRepository.GetCovidCountsAsync(isRefresh);
        }
        [HttpGet("Resource")]
        public IEnumerable<Resource> GetResource()
        {
            return _covidRepository.GetResource();
        }
        //[HttpGet("News")]
        //public CovidNews GetNews()
        //{
        //    return _covidRepository.GetNews();
        //}
        [HttpGet("InfoGraphics")]
        public IEnumerable<InfoGraphic> GetInfoGraphics()
        {
            return _covidRepository.GetInfoGraphics();
        }
    }
}
