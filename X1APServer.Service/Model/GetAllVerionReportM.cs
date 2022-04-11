using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetAllVerionReportM
    {
        public class GetAllVersionReportReq : REQBase
        {
            /// <summary>
            /// Report 類別
            /// </summary>
            public string Category { get; set; }
        }

        public class GetAllVersionReportRsp : RSPBase
        {
            /// <summary>
            /// 所有 Report
            /// </summary>
            public GetAllReportMainM.ReportMain[] Data { get; set; }            
        }

    }
}
