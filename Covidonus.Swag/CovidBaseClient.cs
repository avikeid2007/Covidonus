namespace Covidonus.Swag
{
    public class CovidBaseClient
    {
#if NETFX_CORE
        //public string BaseUrl { get; set; } = "http://covidonusapi.avnishkumar.co.in/";
#else
        public string BaseUrl { get; set; } = "http://localhost:44312/";
#endif
    }
}
