using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidonusApi.Models
{

    public class DailyCovidUpdate
    {
        public IList<CasesTimeSeries> Cases_Time_Series { get; set; }
        public IList<StateWiseData> Statewise { get; set; }
        public IList<Tested> Tested { get; set; }
    }
}