using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetExportReportListLazyM
    {
        public class GetExportReportListLazyReq : REQBase
        {
            /// <summary>
            /// 個案ID(空則抓全部)
            /// </summary>
            public Nullable<int> PID { get; set; }
            /// <summary>
            /// Report 類別
            /// </summary>
            public string ReportCategory { get; set; }
            /// <summary>
            /// 篩選檔案名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 一頁資料筆數
            /// </summary>
            [Range(0, int.MaxValue)]
            public int RowInPage { get; set; } = 30;
            /// <summary>
            /// 頁數
            /// </summary>
            [Range(0, int.MaxValue)]
            public Nullable<int> Page { get; set; }
        }

        public class GetExportReportListLazyRsp : RSPBase
        {
            /// <summary>
            /// 匯出Report清單
            /// </summary>
            public IEnumerable<ExportReport> ExportReportList { get; set; }
        }

        public class ExportReport
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// GUID
            /// </summary>
            public string GUID { get; set; }
            /// <summary>
            /// 檔案名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// Mime Type
            /// </summary>
            public string MimeType { get; set; }
            /// <summary>
            /// 新增時間
            /// </summary>
            public System.DateTime CreateDate { get; set; }
            /// <summary>
            /// 新增人員
            /// </summary>
            public string CreateMan { get; set; }
            /// <summary>
            /// 更新時間
            /// </summary>
            public System.DateTime ModifyDate { get; set; }
            /// <summary>
            /// 更新人員
            /// </summary>
            public string ModifyMan { get; set; }
        }
    }
}
