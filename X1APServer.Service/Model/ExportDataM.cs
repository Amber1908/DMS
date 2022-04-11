using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class ExportDataM
    {
        public class ExportDataReq : REQBase
        {
            /// <summary>
            /// 要匯出資料的 Report ID
            /// </summary>
            public int ID { get; set; }
        }

        public class ExportDataRsp : RSPBase
        {
            /// <summary>
            /// 產出的Excel檔案路徑
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 傳送給前端的顯示名稱
            /// </summary>
            public string FileName { get; set; }
        }
    }
}
