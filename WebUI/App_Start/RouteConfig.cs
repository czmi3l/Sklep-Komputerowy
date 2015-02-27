using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "",
                defaults: new { controller = "Product", action = "Index", page = 1, category = (string)null }
            );

            routes.MapRoute(
                null,
                "Page{page}",
                new { controller = "Product", action = "Index", category = (string)null },
                new { page = @"\d+"}
            );

            routes.MapRoute(
                null,
                "{category}",
                new { controller = "Product", action = "Index", page = 1 },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                name: null,
                url: "{category}/Page{page}",
                defaults: new { controller = "Product", action = "Index" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}"
            );
        }
    }
}
