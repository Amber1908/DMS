using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1AddMorphDataM
    {
        public class X1AddMorphDataReq : REQBase
        {
            /// <summary>
            /// 檢體號碼
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string SampleNo { get; set; }
            /// <summary>
            /// 送檢單位
            /// </summary>
            [MaxLength(100)]
            public string MorphFrom { get; set; }
            /// <summary>
            /// 身分證號
            /// </summary>
            [MaxLength(100)]
            public string IDNo { get; set; }
            /// <summary>
            /// 收件時間
            /// </summary>
            [Required]
            public DateTime RecvDate { get; set; }
            /// <summary>
            /// 使用者姓名
            /// </summary>
            [Required]
            [MaxLength(30)]
            public string AccName { get; set; }
            /// <summary>
            /// 圖片相對路徑
            /// </summary>
            [Required]
            public string[] SampleFolderPath { get; set; }
            /// <summary>
            /// 是否匯出AI
            /// </summary>
            [Required]
            public bool AIFlag { get; set; }
            /// <summary>
            /// 是否匯出Report
            /// </summary>
            [Required]
            public bool ReportFlag { get; set; }
        }
        
        public class X1AddMorphDataRsp : RSPBase
        {
            /// <summary>
            /// Main ID
            /// </summary>
            public int MainID { get; set; }
        }
    }
}
