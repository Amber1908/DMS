using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class ExportExcelM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 要匯出的表單ID
            /// </summary>
            public List<int> ReportMainIDs { get; set; }
            /// <summary>
            /// 開始日期
            /// </summary>
            public DateTime? StartDate { get; set; }
            /// <summary>
            /// 結束日期
            /// </summary>
            public DateTime? EndDate { get; set; }
            /// <summary>
            /// 是否取最新一筆資料
            /// </summary>
            public bool LatestRecord { get; set; }
            /// <summary>
            /// 要匯出的個案ID
            /// </summary>
            public List<int> PatientIDs { get; set; }
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
