using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using WebApplication1.Infrastructure.Filters;

namespace X1APServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 忽略Serializable屬性
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                    new DefaultContractResolver { IgnoreSerializableAttribute = true };
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Main", id = System.Web.Http.RouteParameter.Optional }
            );

            config.Filters.Add(new LogAttribute());
            config.Filters.Add(new ExceptionLogAttribute());
        }
    }
}
