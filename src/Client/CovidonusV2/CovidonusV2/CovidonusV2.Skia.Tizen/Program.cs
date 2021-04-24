using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace CovidonusV2.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new CovidonusV2.App(), args);
            host.Run();
        }
    }
}
