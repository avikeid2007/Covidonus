using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidonusApiV2.Models
{
    public class Tested
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(12)]
        public string IndividualsTestedPerConfirmedCase { get; set; }
        [StringLength(12)]
        public string PositiveCasesFromSamplesReported { get; set; }
        [StringLength(12)]
        public string SampleReportedToday { get; set; }

        public string Source { get; set; }
        [StringLength(12)]
        public string TestPositivityRate { get; set; }
        [StringLength(12)]
        public string TestsConductedByPrivateLabs { get; set; }
        [StringLength(12)]
        public string TestsPerConfirmedCase { get; set; }
        [StringLength(12)]
        public string TestsPerMillion { get; set; }
        [StringLength(12)]
        public string TotalIndividualsTested { get; set; }
        [StringLength(12)]
        public string TotalPositiveCases { get; set; }
        [StringLength(12)]
        public string TotalSamplesTested { get; set; }
        public DateTime UpdateTimeStamp { get; set; }
    }
}