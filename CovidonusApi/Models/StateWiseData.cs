using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidonusApi.Models
{
    public class StateWiseData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string State { get; set; }
        [Required]
        [StringLength(5)]
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
        [StringLength(50)]
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
        public DateTime? TodayUpdatedtime { get; set; }
        public ICollection<DistrictWiseData> DistrictData { get; set; }
    }
}