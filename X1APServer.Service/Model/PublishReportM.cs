using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class PublishReportM
    {
        public class PublishReportReq : REQBase
        {
            /// <summary>
            /// Report Main ID
            /// </summary>
            public int ID { get; set; }
        }

        public class PublishReportRsp : RSPBase
        {

        }
    }
}
