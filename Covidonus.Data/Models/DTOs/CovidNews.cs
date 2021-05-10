using NewsAPI.Models;
using System.Collections.Generic;

namespace Covidonus.Data.Models.DTOs
{
    public class CovidNews
    {
        public int TotalResults
        {
            get;
            set;
        }
        public List<Article> Articles
        {
            get;
            set;
        }
    }
}