using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1ResetReportStateM
    {
        public class X1ResetReportStateReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            public int MainID { get; set; }
        }

        public class X1ResetReportStateRsp : RSPBase
        {

        }
    }
}
