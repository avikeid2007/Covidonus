using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CovidonusApi.Models.DTOs
{
    public class Delta
    {
        public int Id { get; set; }
        public int Confirmed { get; set; }
        public int Deceased { get; set; }
        public int Recovered { get; set; }
        public int Tested { get; set; }
    }
    public class StateTotal
    {
        public Delta Delta { get; set; }
        public Meta Meta { get; set; }
        public Total Total { get; set; }
    }
    public class Meta
    {
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
        public string Population { get; set; }
        public string Notes { get; set; }
    }
    public class Total
    {
        public string Confirmed { get; set; }
        public string Deceased { get; set; }
        public string Recovered { get; set; }
        public string Tested { get; set; }
    }
    public class Daily
    {
        public Dictionary<string, StateTotal> Districts { get; set; }
        public Meta Meta { get; set; }
        public Total Total { get; set; }
        public Delta Delta { get; set; }
    }
}
