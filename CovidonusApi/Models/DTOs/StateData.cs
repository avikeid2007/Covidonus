using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CovidonusApi.Models.DTOs
{
    public class StateData
    {
        [Key]
        public int Id { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public int Active { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
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
        public DateTime? TodayUpdatedtime { get; set; }
        public ICollection<DistrictData> DistrictData { get; set; }

    }
}