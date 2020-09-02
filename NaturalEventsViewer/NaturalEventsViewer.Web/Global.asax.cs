using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NaturalEventsViewer.Web
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            SetSecurityProtocol();

            AreaRegistration.RegisterAllAreas();

            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.MissingMemberHandling = MissingMemberHandling.Error;
            settings.NullValueHandling = NullValueHandling.Include;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }

        public void SetSecurityProtocol()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = (
                        System.Net.SecurityProtocolType.Ssl3
                        | System.Net.SecurityProtocolType.Tls12
                        | System.Net.SecurityProtocolType.Tls11
                        | System.Net.SecurityProtocolType.Tls
                    );
            }
            catch (Exception ex)
            {
                // TODO: log
                throw;
            }
        }
    }
}
