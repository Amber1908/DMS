using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddReportMainFileM
    {
        public class AddReportMainFileReq : REQBase
        {
            /// <summary>
            /// Report Main ID
            /// </summary>
            public int RMID { get; set; }
            /// <summary>
            /// Report Quest ID
            /// </summary>
            public int RQID { get; set; }
            /// <summary>
            /// 伺服器上檔案路徑
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 檔案名稱
            /// </summary>
            public string FileName { get; set; }

            public string MimeType { get; set; }
        }

        public class AddReportMainFileRsp : RSPBase
        {

        }
    }
}
