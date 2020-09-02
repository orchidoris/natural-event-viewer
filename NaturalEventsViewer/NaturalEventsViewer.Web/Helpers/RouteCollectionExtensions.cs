using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace NaturalEventsViewer.Web.Helpers
{
    public static class RouteCollectionExtensions
    {
        /// <summary>
        /// Beta-urls prefix
        /// </summary>
        public readonly static string Beta = "beta";

        /// <summary>
        /// DataTokens key for namespaces
        /// </summary>
        public const string DataTokensNamespacesKey = "Namespaces";

        /// <summary>
        /// Extension to convert URLs to lowercase.  Replaces typical MapRoute() method
        /// </summary>
        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return routes.MapRouteLowercase(name, url, defaults, null, namespaces);
        }

        /// <summary>
        /// Extension to convert URLs to lowercase.  Replaces typical MapRoute() method
        /// </summary>
        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults = null, object constraints = null, string[] namespaces = null, bool isBeta = false)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (isBeta)
            {
                url = String.Join("/", new[] { Beta, url });
                name = String.Concat(Beta, name);
            }
            var route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(),
                UseTrailingSlash = url.EndsWith(LowercaseRoute.Slash)
            };

            if (namespaces != null && namespaces.Length > 0)
            {
                route.DataTokens[DataTokensNamespacesKey] = namespaces;
            }
            routes.Add(name, route);
            return route;
        }
    }
}