using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace X1APServer.Service.Misc
{
    public class CustomUriBuilder : UriBuilder
    {
        private readonly NameValueCollection _params;

        public CustomUriBuilder(string uri) : base(uri)
        {
            _params = HttpUtility.ParseQueryString(Query);
        }

        public void AddOrUpdateParam(string key, string value)
        {
            _params[key] = value;
            Query = _params.ToString();
        }

        public void RemoveParam(string key)
        {
            _params.Remove(key);
            Query = _params.ToString();
        }


    }
}
