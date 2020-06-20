using System.Collections.Generic;
using System.Web.Http;

namespace CovidonusApi.Controllers
{
    public class StateController : ApiController
    {
        // GET api/State
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
