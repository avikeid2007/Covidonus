using System;
using System.Collections.Generic;

namespace Covidonus.Data.Models
{
    public class StateWiseData
    {

        public int Id { get; set; }

        public string State { get; set; }
        public string StateCode { get; set; }
        public int Active { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Tested { get; set; }
        public int Recovered { get; set; }
        public int DeltaActive { get; set; }
        public int DeltaConfirmed { get; set; }
        public int DeltaDeaths { get; set; }
        public int DeltaRecovered { get; set; }
        public string StateNotes { get; set; }
        public DateTime? LastUpdatedtime { get; set; }
        public string StateLogo { get; set; }
        public int TodayConfirmed { get; set; }
        public int TodayDeaths { get; set; }
        public int TodayRecovered { get; set; }
        public int TodayTested { get; set; }
        public string Population { get; set; }
        public string TestSource { get; set; }
        public int TestPerMillion { get; set; }
        public string TotalPositiveCases { get; set; }
        public string RecoverRate { get; set; }
        public string DeathRate { get; set; }
        public string WhatsAppBot { get; set; }
        public string FacebookBot { get; set; }
        public string EPassLink { get; set; }
        public DateTime? TodayUpdatedtime { get; set; }
        public ICollection<DistrictWiseData> DistrictData { get; set; }
    }
}