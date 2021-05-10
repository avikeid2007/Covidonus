using System;


namespace Covidonus.Data.Models
{
    public class Tested
    {

        public int Id { get; set; }
        public string IndividualsTestedPerConfirmedCase { get; set; }
        public string PositiveCasesFromSamplesReported { get; set; }
        public string SampleReportedToday { get; set; }

        public string Source { get; set; }

        public string TestPositivityRate { get; set; }

        public string TestsConductedByPrivateLabs { get; set; }

        public string TestsPerConfirmedCase { get; set; }

        public string TestsPerMillion { get; set; }

        public string TotalIndividualsTested { get; set; }

        public string TotalPositiveCases { get; set; }

        public string TotalSamplesTested { get; set; }
        public DateTime UpdateTimeStamp { get; set; }
    }
}