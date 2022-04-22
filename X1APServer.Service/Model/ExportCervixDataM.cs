using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class ExportCervixDataM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 開始日期
            /// </summary>
            public DateTime? StartDate { get; set; }
            /// <summary>
            /// 結束日期
            /// </summary>
            public DateTime? EndDate { get; set; }
            /// <summary>
            /// 確診開始日期
            /// </summary>
            public DateTime? DiagnosedstartDate { get; set; }
            /// <summary>
            /// 確診結束日期
            /// </summary>
            public DateTime? DiagnosedendDate { get; set; }
            /// <summary>
            /// 表單狀態
            /// </summary>
            /// <summary>
            /// 表單狀態
            /// </summary>
            public int Status { get; set; }

        }

        public class Response : RSPBase
        {
            /// <summary>
            /// Excel Url
            /// </summary>
            public string ExcelUrl { get; set; }
        }
    }
}
