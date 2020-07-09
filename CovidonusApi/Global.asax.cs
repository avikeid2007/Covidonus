using CovidonusApi.Models;
using CovidonusApi.Scheduler;
using NSwag.AspNet.Owin;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CovidonusApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapOwinPath("swagger", app =>
            {
                app.UseSwaggerUi3(typeof(WebApiApplication).Assembly, settings =>
                {
                    settings.MiddlewareBasePath = "/swagger";
                    settings.DocumentTitle = "Covidonus api";
                    settings.GeneratorSettings.DefaultUrlTemplate = "api/{controller}/{action}/{id}";
                });
            });
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<CovidonusContext>(new DropCreateDatabaseIfModelChanges<CovidonusContext>());
            JobScheduler.StartAsync();
        }
    }
}
