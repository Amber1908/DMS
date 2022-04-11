using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetAllReportMainM
    {
        public class GetAllReportMainReq : REQBase
        {
            /// <summary>
            /// 是否已發佈
            /// </summary>
            public bool? isPublish { get; set; }

        }

        public class GetAllReportMainRsp : RSPBase
        {
            /// <summary>
            /// Report Main清單
            /// </summary>
            public IEnumerable<ReportMain> ReportMainList { get; set; }
        }

        public class ReportMain
        {
            /// <summary>
            /// Report Main ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// Report 類別
            /// </summary>
            public string Category { get; set; }
            /// <summary>
            /// Report 標題
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// Report 描述
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// 新增時間
            /// </summary>
            public DateTime CreateDate { get; set; }
            /// <summary>
            /// 新增人員
            /// </summary>
            public string CreateMan { get; set; }
            /// <summary>
            /// 修改時間
            /// </summary>
            public DateTime ModifyDate { get; set; }
            /// <summary>
            /// 修改人員
            /// </summary>
            public string ModifyMan { get; set; }
            /// <summary>
            /// 發佈時間
            /// </summary>
            public DateTime? PublishDate { get; set; }
            /// <summary>
            /// 是否發佈
            /// </summary>
            public bool IsPublish { get; set; }
            /// <summary>
            /// 功能代碼
            /// </summary>
            public string FuncCode { get; set; }
            /// <summary>
            /// 報告版本清單
            /// </summary>
            public List<ReportVersionItem> ReportVersionList { get; set; }
        }

        public class ReportVersionItem
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 發佈時間
            /// </summary>
            public DateTime? PublishDate { get; set; }
        }
    }
}
