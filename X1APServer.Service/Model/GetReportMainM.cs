using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetReportMainM
    {
        public class GetReportMainReq : REQBase
        {
            public int? ID { get; set; }
            /// <summary>
            /// Report Answer Main ID
            /// </summary>
            public int? ReportAnsMID { get; set; }
            /// <summary>
            /// Report 類別
            /// </summary>
            public string ReportCategory { get; set; }
            /// <summary>
            /// 是否為發布狀態
            /// </summary>
            public bool? IsPublish { get; set; }
        }

        public class GetReportMainRsp : RSPBase
        {
            /// <summary>
            /// Report Main
            /// </summary>
            public ReportMain Data { get; set; }
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
            /// 問卷結構 json
            /// </summary>
            public string OutputJson { get; set; }
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
            /// 是否已發佈
            /// </summary>
            public bool isPublish { get; set; }
            /// <summary>
            /// 預計發佈時間
            /// </summary>
            public DateTime ReserveDate { get; set; }
            /// <summary>
            /// 問卷排序值
            /// </summary>
            public int IndexNum { get; set; }
            /// <summary>
            /// 權限代碼
            /// </summary>
            public string FuncCode { get; set; }
        }
    }
}
