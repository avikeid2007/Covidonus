namespace CovidonusApi.Models.DTOs
{
    public class DistrictData
    {

        public int Id { get; set; }
        public string District { get; set; }
        public string Notes { get; set; }
        public int Active { get; set; }
        public int Confirmed { get; set; }
        public int Deceased { get; set; }
        public int Recovered { get; set; }
        public int DeltaId { get; set; }
        public string StateCode { get; set; }
        public int StateWiseDataId { get; set; }
        public Delta Delta { get; set; }

    }
    public class Delta
    {
        public int Id { get; set; }
        public int Confirmed { get; set; }
        public int Deceased { get; set; }
        public int Recovered { get; set; }
    }
}