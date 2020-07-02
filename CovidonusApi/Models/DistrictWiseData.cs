using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidonusApi.Models
{

    public class DistrictWiseData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string District { get; set; }
        public string Notes { get; set; }
        public int Active { get; set; }
        public int Confirmed { get; set; }
        public int Deceased { get; set; }
        public int Recovered { get; set; }
        public int DeltaId { get; set; }
        [Required]
        [StringLength(5)]
        public string StateCode { get; set; }
        [Required]
        public int StateWiseDataId { get; set; }
        public int TodayConfirmed { get; set; }
        public int TodayDeaths { get; set; }
        public int TodayRecovered { get; set; }
        public int TodayTested { get; set; }
        public string Population { get; set; }
        public DateTime? TodayUpdatedtime { get; set; }
        public StateWiseData StateWiseData { get; set; }
        public virtual DeltaData Delta { get; set; }
    }


}