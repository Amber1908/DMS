using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddReportAnsFileM
    {
        public class AddReportAnsFileReq : REQBase
        {
            /// <summary>
            /// Answer Main ID
            /// </summary>
            public int AnswerMID { get; set; }
            /// <summary>
            /// 問題 ID
            /// </summary>
            public int QuestionID { get; set; }
            /// <summary>
            /// 檔案名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 檔案路徑
            /// </summary>
            public string FilePath { get; set; }

            public string MimeType { get; set; }
        }

        public class AddReportAnsFileRsp : RSPBase
        {

        }
    }
}
