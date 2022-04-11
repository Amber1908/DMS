using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class DeleteReportMainM
    {
        public class DeleteReportMainReq : REQBase
        {
            /// <summary>
            /// 要刪除的Report Main ID
            /// </summary>
            public int ID { get; set; }
        }

        public class DeleteReportMainRsp : RSPBase
        {

        }
    }
}
