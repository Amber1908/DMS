using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetReportM
    {
        public class GetReportReq : REQBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            public int ID { get; set; }
        }

        public class GetReportRsp : RSPBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// Report 是否已鎖定
            /// </summary>
            public bool Lock { get; set; }
            /// <summary>
            /// Report 狀態
            /// </summary>
            public int Status { get; set; }
            /// <summary>
            /// 批號
            /// </summary>
            public string SequenceNum { get; set; }
            /// <summary>
            /// 答案列表
            /// </summary>
            public List<ReportAnswerD> Answers { get; set; }
            /// <summary>
            /// 檔案列表
            /// </summary>
            public List<ReportAnswerFile> Files { get; set; }
        }

        public class ReportAnswerD
        {
            /// <summary>
            /// 問題ID
            /// </summary>
            public string QuestionID { get; set; }
            /// <summary>
            /// 問題類型
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 答案
            /// </summary>
            public string Value { get; set; }
        }

        public class ReportAnswerFile
        {
            /// <summary>
            /// 問題ID
            /// </summary>
            public string QuestionID { get; set; }
            /// <summary>
            /// 問題類型
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 檔案URL
            /// </summary>
            public string FileURL { get; set; }
        }
    }
}
