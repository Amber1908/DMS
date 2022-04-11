using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApplication1.Infrastructure.Utility
{
    public static class UriUtility
    {
        public static string GetBaseUrl(HttpRequestMessage request, string relativePath)
        {
            bool ignorePort;
            string url = "";
            if (!bool.TryParse(ConfigurationManager.AppSettings["IgnoreRequestPort"], out ignorePort))
            {
                throw new Exception("IgnoreRequestPort 格式錯誤");
            }

            if (ignorePort)
            {
                url = string.Format("{0}://{1}{2}", request.RequestUri.Scheme, request.RequestUri.Host, VirtualPathUtility.ToAbsolute(relativePath));
            }
            else
            {
                url = request.RequestUri.GetLeftPart(UriPartial.Authority) +
                    VirtualPathUtility.ToAbsolute(relativePath);
            }

            return url;
        }
    }
}