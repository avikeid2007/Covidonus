using CovidonusApi.Models.DTOs;
using CovidonusApi.Repositories.Abstraction;
using System.Collections.Generic;
using System.Web.Http;

namespace CovidonusApi.Controllers
{
    public class StateController : ApiController
    {
        ICovidRepository _covidRepository;
        public StateController(ICovidRepository covidRepository)
        {
            _covidRepository = covidRepository;
        }
        // GET api/State
        public IEnumerable<StateData> Get()
        {
            return _covidRepository.GetStates();
        }
    }
}
