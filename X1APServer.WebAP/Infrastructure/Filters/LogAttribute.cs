using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApplication1.Infrastructure.Common;

namespace WebApplication1.Infrastructure.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public static readonly string REQUEST_TIME = "RequestTime";
        private readonly Logger logger = LogManager.GetLogger("WebLogger");

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!LogUtils.SkipLogging(actionContext))
            {
                // 紀錄收到 Request 時間
                actionContext.Request.Properties[REQUEST_TIME] = DateTime.Now.ToString();
            }

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            object requestTime = null;
            if (actionExecutedContext.Exception == null && actionExecutedContext.Request.Properties.TryGetValue(REQUEST_TIME, out requestTime))
            {
                string requestTimeStr = requestTime.ToString();
                HttpRequest request = HttpContext.Current.Request;
                StringBuilder logMessage = new StringBuilder();
                logMessage.AppendLine();
                logMessage.AppendLine("Request IP: " + request.UserHostAddress);
                logMessage.AppendLine("Browser Info: " + request.Browser.Browser + "-" + request.Browser.Version + "-" + request.Browser.Type);
                logMessage.AppendLine("Receive Time: " + requestTimeStr);
                if (!LogUtils.IgnoreRequestContent(actionExecutedContext)) logMessage.AppendLine("Receive Content: " + HttpContextUtils.GetRequestContent(actionExecutedContext));
                logMessage.AppendLine("Response Time: " + DateTime.Now.ToString());
                if (!LogUtils.IgnoreResponseContent(actionExecutedContext)) logMessage.AppendLine("Response Content: " + HttpContextUtils.GetResponseContent(actionExecutedContext));
                logger.Debug(logMessage.ToString());
            }

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }


    }
}