namespace Covidonus.Swag
{
    public class CovidBaseClient
    {
        //#if !NETFX_CORE
        //        public string BaseUrl { get; set; } = "http://covidonusapi.avnishkumar.co.in/";
        //#else
        public static string DefaultBaseUrl { get; set; } = "http://covidonus.avnishkumar.co.in/";
        //#endif
    }
}
