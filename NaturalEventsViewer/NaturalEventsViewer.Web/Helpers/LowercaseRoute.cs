using System;
using System.Web.Routing;

namespace NaturalEventsViewer.Web.Helpers
{
    //
    // based on: http://www.babel-lutefisk.net/2011/11/mvc-lower-case-urls.html
    //
    public class LowercaseRoute : Route
    {
        public const string Slash = "/";

        public bool UseTrailingSlash { get; set; }

        public LowercaseRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler) { }

        public LowercaseRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler) { }

        public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler) { }

        public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler) { }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            VirtualPathData path = base.GetVirtualPath(requestContext, values);

            if (path != null)
            {
                string virtualPath = path.VirtualPath;
                int lastIndexOfUrl = virtualPath.LastIndexOf("?", System.StringComparison.Ordinal);
                if (lastIndexOfUrl > 0)
                {
                    string leftPart = virtualPath.Substring(0, lastIndexOfUrl).ToLowerInvariant();
                    string queryPart = virtualPath.Substring(lastIndexOfUrl);
                    path.VirtualPath = leftPart + queryPart;
                }
                else
                {
                    path.VirtualPath = path.VirtualPath.ToLowerInvariant();
                }
                if (UseTrailingSlash && !String.IsNullOrWhiteSpace(path.VirtualPath) && !path.VirtualPath.EndsWith(Slash))
                {
                    path.VirtualPath += Slash;
                }
            }

            return path;
        }
    }
}