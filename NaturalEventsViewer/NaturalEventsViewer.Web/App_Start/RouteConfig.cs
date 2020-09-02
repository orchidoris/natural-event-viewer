using NaturalEventsViewer.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace NaturalEventsViewer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(name: "Root", url: "",
                defaults:
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "JA.CampaignManager.Web.Controllers" });

            routes.MapRouteLowercase(name: "Home", url: "home/{action}/{id}",
                defaults:
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "JA.CampaignManager.Web.Controllers" });

            // This should be the last route
            // Don't add any routes below this route (only above)
            routes.MapRoute(name: "spa-fallback", url: "{*.}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                },
                namespaces: new[] { "JA.CampaignManager.Web.Controllers" });
        }
    }
}
