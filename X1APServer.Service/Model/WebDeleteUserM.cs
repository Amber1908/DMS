using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class WebDeleteUserM
    {
        public class WebDeleteUserReq : REQBase
        {
            /// <summary>
            /// 要刪除的帳號
            /// </summary>
            public string RequestedAccID { get; set; }
        }

        public class WebDeleteUserRsp : RSPBase
        {

        }
    }
}
