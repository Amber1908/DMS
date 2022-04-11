using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace X1APServer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "File",
                url: "File/{id}",
                defaults: new { controller = "File", action = "Get" }
            );

            routes.MapRoute(
                name: "PDFFile",
                url: "ReportPDF/{id}",
                defaults: new { controller = "Home", action = "ReportPDF" }
            );

            routes.MapRoute(
                name: "ReportViewer",
                url: "ReportViewer/{pid}",
                defaults: new { controller = "Home", action = "ReportViewer" }
            );

            routes.MapRoute(
                name: "PlayGround",
                url: "PlayGround",
                defaults: new { controller = "Home", action = "PlayGround" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{*pathInfo}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
