using CovidonusApi.Repositories;
using CovidonusApi.Repositories.Abstraction;
using CovidonusApi.Unity;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace CovidonusApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<ISeedDataRepository, SeedDataRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICovidRepository, CovidRepository>(new ContainerControlledLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
