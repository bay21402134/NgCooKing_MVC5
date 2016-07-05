using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NgCooking_Asp.net
{
    public class RouteConfig
    {
        public static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Recettes", action = "Home", id = UrlParameter.Optional }
            );

           // routes.MapRoute(
           //    name: "Communautes",
           //    url: "{controller}/{action}/{id}"
           // );

           // routes.MapRoute(
           //   name: "Ingredients",
           //   url: "{controller}/{action}/{id}"
           //);

        }
    }
}
