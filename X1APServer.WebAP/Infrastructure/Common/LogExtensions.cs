using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Routing;

namespace WebApplication1.Infrastructure.Common
{
    [LayoutRenderer("aspnetmvc-controller")]
    public class ControllerLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, NLog.LogEventInfo logEvent)
        {
            var controllerName = string.Empty;
            if (HttpContext.Current != null)
            {
                //controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
                var routeData = HttpContext.Current.Request.RequestContext.RouteData;

                if (routeData.Values["controller"] != null)
                {
                    string subroutes = routeData.Values["controller"].ToString();
                    controllerName = subroutes == null ? "Common" : subroutes;
                 }
                else
                {
                    var subroutes = (IEnumerable<IHttpRouteData>)routeData.Values["MS_SubRoutes"];
                    controllerName = subroutes == null ? "Common" : (subroutes.First().Route.RouteTemplate.ToString().Split('/'))[0];
                }


            }

            builder.Append(controllerName);
        }
    }

    [LayoutRenderer("aspnetmvc-action")]
    public class ActionLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, NLog.LogEventInfo logEvent)
        {
            var actionName = string.Empty;
            if (HttpContext.Current != null)
            {
                //actionName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                var routeData = HttpContext.Current.Request.RequestContext.RouteData;

                if (routeData.Values["controller"] != null)
                {
                    string subroutes = routeData.Values["action"].ToString();
                    actionName = subroutes == null ? "Common" : subroutes;
                }
                else
                {
                    var subroutes = (IEnumerable<IHttpRouteData>)routeData.Values["MS_SubRoutes"];
                    actionName = subroutes == null ? "Common" : (subroutes.First().Route.RouteTemplate.ToString().Split('/'))[1];
                }
            }

            builder.Append(actionName);
        }
    }
}