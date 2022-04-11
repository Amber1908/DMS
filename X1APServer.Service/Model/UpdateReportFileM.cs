using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateReportFileM
    {
        public class UpdateReportFileReq : REQBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            public int ReportID { get; set; }
            /// <summary>
            /// 問題 ID
            /// </summary>
            public string QuestionID { get; set; }
            /// <summary>
            /// 檔案路徑
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 原始檔案名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 檔案 MimeType
            /// </summary>
            public string MimeType { get; set; }
        }

        public class UpdateReportFileRsp : RSPBase
        {

        }
    }
}
