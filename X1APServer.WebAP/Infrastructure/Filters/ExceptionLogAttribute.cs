using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using WebApplication1.Infrastructure.Common;
using X1APServer.Service.Model;

namespace WebApplication1.Infrastructure.Filters
{
    public class ExceptionLogAttribute : ExceptionFilterAttribute
    {
        private readonly Logger logger = LogManager.GetLogger("WebErrorLogger");

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            HttpRequest request = HttpContext.Current.Request;
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine();
            logMessage.AppendLine("Request IP: " + request.UserHostAddress);
            logMessage.AppendLine("Browser Info: " + request.Browser.Browser + "-" + request.Browser.Version + "-" + request.Browser.Type);

            object requestTime = null;
            if (actionExecutedContext.Request.Properties.TryGetValue(LogAttribute.REQUEST_TIME, out requestTime))
            {
                string requestTimeStr = requestTime.ToString();
                logMessage.AppendLine("Receive Time: " + requestTimeStr);
            }

            if (!LogUtils.IgnoreRequestContent(actionExecutedContext)) logMessage.AppendLine("Receive Content: " + HttpContextUtils.GetRequestContent(actionExecutedContext));
            logMessage.AppendLine("Exception : " + actionExecutedContext.Exception.ToString());

            var entityException = actionExecutedContext.Exception as DbEntityValidationException;
            if (entityException != null)
            {
                logMessage.AppendLine(string.Format("Validation errors:{0}", string.Join(Environment.NewLine, entityException.EntityValidationErrors.Select(e => string.Join(Environment.NewLine, e.ValidationErrors.Select(v => string.Format("{0} - {1}", v.PropertyName, v.ErrorMessage)))))));
            }
            

            logger.Error(logMessage.ToString());

            var result = new RSPBase()
            {
                ReturnCode = ErrorCode.ServerError,
                ReturnMsg = actionExecutedContext.Exception.Message + "，請聯絡管理員"
            };
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(result);
            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }
    }
}