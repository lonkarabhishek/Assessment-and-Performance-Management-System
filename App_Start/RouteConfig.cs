using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mini_Tekstac_Question_Paper_Generation
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
         
            routes.MapRoute(
                      "Catchall",
                       "{*catchall}", // This is a wildcard routes
                         new { controller = "Home", action = "Lost" }
                            );
        }
    }
}
