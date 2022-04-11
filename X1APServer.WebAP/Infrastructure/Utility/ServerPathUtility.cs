using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApplication1.Infrastructure.Utility
{
    public static class ServerPathUtility
    {
        public static string GetVirtualPath(HttpRequestMessage request, string relativePath)
        {
            string rootPath = request.GetRequestContext().VirtualPathRoot;
            string url = (rootPath + relativePath).Replace("//", "/");
            return url;
        }
    }
}