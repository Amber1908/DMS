using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace WebApplication1.Infrastructure.Common
{
    public class HttpContextUtils
    {
        /// <summary>
        /// 取得 Request Content
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static string GetRequestContent(HttpActionExecutedContext actionContext)
        {
            var stream = actionContext.Request.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            // 不能關閉 stream 其他 function 也要讀取
            var reader = new StreamReader(stream, encoding);
            stream.Position = 0;
            string result = reader.ReadToEnd().Replace("\n", "").Replace("\t", "").Replace("\r", "");
            // 把 stream 位置重置，讓其他 function 讀取
            stream.Position = 0;

            return result;
        }

        /// <summary>
        /// 取得 Response Content
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static string GetResponseContent(HttpActionExecutedContext actionContext)
        {
            var stream = actionContext.Response.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            var reader = new StreamReader(stream, encoding);
            stream.Position = 0;
            string result = reader.ReadToEnd().Replace("\n", "").Replace("\t", "").Replace("\r", "");
            stream.Position = 0;

            return result;
        }
    }
}