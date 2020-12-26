using System.Collections.Generic;

namespace Covidonus.Data.Models
{

    public class DailyCovidUpdate
    {
        public IList<CasesTimeSeries> Cases_Time_Series { get; set; }
        public IList<StateWiseData> Statewise { get; set; }
        public IList<Tested> Tested { get; set; }
    }
}