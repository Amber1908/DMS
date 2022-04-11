using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddGeneralReportM
    {
        public class AddGeneralReportReq : REQBase
        {
            /// <summary>
            /// 醫令ID
            /// </summary>
            [Required]
            public int OID { get; set; }
            /// <summary>
            /// 個案ID
            /// </summary>
            [Required]
            public Nullable<int> PID { get; set; }
            /// <summary>
            /// Report Main ID
            /// </summary>
            [Required]
            public int ID { get; set; }
            /// <summary>
            /// 填寫日期
            /// </summary>
            public DateTime? FillingDate { get; set; }
            /// <summary>
            /// 表單狀態
            /// </summary>
            public int? Status { get; set; }
            /// <summary>
            /// 批號
            /// </summary>
            //public string SequenceNum { get; set; }
            /// <summary>
            /// 答案列表
            /// </summary>
            [Required]
            public List<UpdateReportM.ReportAnswerD> Answers { get; set; }
        }

        public class AddGeneralReportRsp : RSPBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            public int ReportID { get; set; }
        }
    }
}
