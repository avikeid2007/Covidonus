namespace CovidonusV2.Swag
{
    public class CovidBaseClient
    {
        //#if !NETFX_CORE
        //        public string BaseUrl { get; set; } = "http://covidonusapi.avnishkumar.co.in/";
        //#else
        //public string BaseUrl { get; set; } = "http://localhost:44313/";
        public string BaseUrl { get; set; } = "http://covidonus.avnishkumar.co.in/";
        //#endif
    }
}
