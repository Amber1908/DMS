using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class ExportCodingBookM
    {
        public class ExportCodingBookReq : REQBase
        {
            /// <summary>
            /// Report Main ID
            /// </summary>
            [System.ComponentModel.DataAnnotations.Required]
            public string ReportMID { get; set; }
            /// <summary>
            /// 是否發佈
            /// </summary>
            public bool IsPublish { get; set; }
        }

        public class ExportCodingBookRsp : RSPBase
        {
            /// <summary>
            /// 產出的Excel檔案路徑
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 傳送給前端的顯示名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 下載檔案路徑
            /// </summary>
            public string ExcelUrl { get; set; }
        }
    }
}
