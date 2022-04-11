using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetReportsM
    {
        public class GetReportsReq : REQBase
        {
            /// <summary>
            /// 身份證號
            /// </summary>
            [Required]
            public string IDNo { get; set; }
            /// <summary>
            /// Report 類別
            /// </summary>
            [Required]
            public string Category { get; set; }
        }

        public class GetReportsRsp : RSPBase
        {
            /// <summary>
            /// Report 清單
            /// </summary>
            public IEnumerable<Report> Reports { get; set; }
        }

        public class Report
        {
            /// <summary>
            /// Report ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 填寫日期
            /// </summary>
            public DateTime FillingDate { get; set; }
            /// <summary>
            /// 建立時間
            /// </summary>
            public System.DateTime CreateDate { get; set; }
            /// <summary>
            /// 建立人員
            /// </summary>
            public string CreateMan { get; set; }
            /// <summary>
            /// 修改時間
            /// </summary>
            public System.DateTime ModifyDate { get; set; }
            /// <summary>
            /// 修改人員
            /// </summary>
            public string ModifyMan { get; set; }
            /// <summary>
            /// 收件日期
            /// </summary>
            public DateTime RecvDate { get; set; }
            /// <summary>
            /// 癌醫檢體編號
            /// </summary>
            public string CCSpecimentID { get; set; }
            /// <summary>
            /// 病人姓名
            /// </summary>
            public string PatientName { get; set; }
            /// <summary>
            /// 匯出日期
            /// </summary>
            public DateTime? ExportDate { get; set; }
            /// <summary>
            /// 匯出醫生
            /// </summary>
            public string ExportDoctor { get; set; }
            /// <summary>
            /// 表單狀態
            /// </summary>
            public int Status { get; set; }
        }
    }
}
