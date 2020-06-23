using System;

namespace CovidonusApi.Models.DTOs
{
    public class DailyTotalCount
    {
        public int Id { get; set; }
        public int DailyConfirmed { get; set; }
        public int DailyDeceased { get; set; }
        public int DailyRecovered { get; set; }
        public int TotalConfirmed { get; set; }
        public int TotalDeceased { get; set; }
        public int TotalRecovered { get; set; }
        public DateTime DateFull { get; set; }
    }
}