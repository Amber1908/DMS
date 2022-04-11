using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class WebLogoutM
    {
        public class WebLogoutReq : REQBase
        {
            /// <summary>
            /// 要登出的帳號
            /// </summary>
            public string RequestedAccID { get; set; }
        }

        public class WebLogoutRsp : RSPBase
        {

        }
    }
}
