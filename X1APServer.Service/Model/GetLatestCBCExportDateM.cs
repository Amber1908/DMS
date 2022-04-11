using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetLatestCBCExportDateM
    {
        public class GetLatestCBCExportDateReq : REQBase
        {
            /// <summary>
            /// Report 答案主表 ID
            /// </summary>
            [Required]
            public int? ReportAnsMID { get; set; }
        }

        public class GetLatestCBCExportDateRsp : RSPBase
        {
            /// <summary>
            /// Convention 匯出時間
            /// </summary>
            public DateTime? CBCExportDate { get; set; }
        }
    }
}
