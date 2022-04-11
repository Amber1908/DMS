using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApplication1.Infrastructure.Filters;

namespace WebApplication1.Infrastructure.Common
{
    public class LogUtils
    {
        /// <summary>
        /// 判斷是否要 Log
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static bool SkipLogging(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<NoLogAttribute>().Any() ||
                actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<NoLogAttribute>().Any();
        }

        /// <summary>
        /// 判斷是否要 Log Request Content
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static bool IgnoreRequestContent(HttpActionExecutedContext actionContext)
        {
            return actionContext.ActionContext.ActionDescriptor.GetCustomAttributes<IgnoreRequestContentAttribute>().Any() ||
                actionContext.ActionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<IgnoreRequestContentAttribute>().Any();
        }

        /// <summary>
        /// 判斷是否要 Log Response Content
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static bool IgnoreResponseContent(HttpActionExecutedContext actionContext)
        {
            return actionContext.ActionContext.ActionDescriptor.GetCustomAttributes<IgnoreResponseContentAttribute>().Any() ||
                actionContext.ActionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<IgnoreResponseContentAttribute>().Any();
        }
    }
}