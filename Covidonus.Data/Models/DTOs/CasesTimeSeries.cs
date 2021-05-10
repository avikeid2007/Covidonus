using System;
using System.Globalization;

namespace Covidonus.Data.Models
{
    public class CasesTimeSeries
    {
        public int Id { get; set; }
        public int DailyConfirmed { get; set; }
        public int DailyDeceased { get; set; }
        public int DailyRecovered { get; set; }
        public string Date { get; set; }
        public int TotalConfirmed { get; set; }
        public int TotalDeceased { get; set; }
        public int TotalRecovered { get; set; }
        public DateTime DateFull
        {
            get => ConvertDate(Date);
        }
        private static DateTime ConvertDate(string date)
        {
            var da = $"{date}{DateTime.Now.Year}";
            CultureInfo provider = CultureInfo.InvariantCulture;
            if (!string.IsNullOrEmpty(date) && DateTime.TryParseExact(da, "dd MMMM yyyy", provider, DateTimeStyles.None, out DateTime newdate))
            {
                return newdate;
            }
            return default(DateTime);
        }
    }
}