using System;

namespace CovidonusApiV2.Models.DTOs
{
    public class DailyTotalCount
    {
        public int Id { get; set; }
        public int DailyConfirmed { get; set; }
        public int DailyDeceased { get; set; }
        public int DailyRecovered { get; set; }
        public int TotalConfirmed { get; set; }
        public int TotalActive { get; set; }
        public int TotalDeceased { get; set; }
        public int TotalRecovered { get; set; }
        public DateTime DateFull { get; set; }
        public string RecoverRatio { get; set; }
        public string DeathRatio { get; set; }
        public string TestedToday { get; set; }
        public string TestedTotal { get; set; }
        public string TestedSource { get; set; }
    }
}