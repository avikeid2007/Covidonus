﻿using NewsAPI.Models;
using System.Collections.Generic;

namespace CovidonusApiV2.Models.DTOs
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