using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddDMSSettingM
    {
        public class Request
        {
            public int Web_sn { get; set; }
            public string Web_name { get; set; }
            public string Web_db { get; set; }
            public Nullable<int> Logo { get; set; }
            public string SessionKey { get; set; }
            public string AccID { get; set; }
        }

        public class Response : RSPBase
        {

        }
    }
}
