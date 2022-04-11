using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class CloseoutReportM
    {
        public class CloseoutReportReq : REQBase
        {
            /// <summary>
            /// Report 答案主表 ID
            /// </summary>
            [Required]
            public int? ReportAnsMID { get; set; }
        }

        public class CloseoutReportRsp : RSPBase
        {

        }
    }
}
