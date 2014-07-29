﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestDrivingASPNetRouting.Tests
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("MixedSegments", "Mixed{controller}/{action}");

            routes.MapRoute("SimpleRoute", "{controller}/{action}", 
                new { controller = "DefaultController", action = "DefaultIndex" });

            routes.MapRoute("Public", "Public/{controller}/{action}");
        }
    }
}
