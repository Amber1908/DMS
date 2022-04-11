using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.DataAnnotaionExt;

namespace X1APServer.Service.Model
{
    public class X1AddAIReportM
    {
        public class X1AddAIReportReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            [Required]
            public int MainID { get; set; }
            /// <summary>
            /// AI 檔絕對位置
            /// </summary>
            [Required]
            [MaxFileName(50)]
            public string AIFilePath { get; set; }
        }


        public class X1AddAIReportRsp : RSPBase
        {
            /// <summary>
            /// AI 資料 ID
            /// </summary>
            public int ID { get; set; }
        }
    }
}
