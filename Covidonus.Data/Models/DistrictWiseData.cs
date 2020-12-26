using System;

namespace Covidonus.Data.Models
{

    public class DistrictWiseData
    {
        public int Id { get; set; }

        public string District { get; set; }
        public string Notes { get; set; }
        public int Active { get; set; }
        public int Confirmed { get; set; }
        public int Deceased { get; set; }
        public int Recovered { get; set; }
        public int DeltaId { get; set; }

        public string StateCode { get; set; }

        public int StateWiseDataId { get; set; }
        public int TodayConfirmed { get; set; }
        public int TodayDeaths { get; set; }
        public int TodayRecovered { get; set; }
        public int TodayTested { get; set; }
        public string Population { get; set; }
        public string RecoverRate { get; set; }
        public string DeathRate { get; set; }
        public DateTime? TodayUpdatedtime { get; set; }
        public StateWiseData StateWiseData { get; set; }
        public virtual DeltaData Delta { get; set; }
    }


}