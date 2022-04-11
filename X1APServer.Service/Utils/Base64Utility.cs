using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Utils
{
    public class Base64Utility
    {
        public static string ToBase64String(string str, string encoding = "utf-8")
        {
            var bytes = Encoding.GetEncoding(encoding).GetBytes(str);
            string base64Str = Convert.ToBase64String(bytes);
            return base64Str;
        }
    }
}
