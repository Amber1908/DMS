using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateReportM
    {
        public class UpdateReportReq : REQBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            [Required]
            public int ID { get; set; }
            /// <summary>
            /// 批號
            /// </summary>
            //[Required(ErrorMessage = "未填批號!")]
            public string SequenceNum { get; set; }
            /// <summary>
            /// 答案列表
            /// </summary>
            [Required]
            public List<ReportAnswerD> Answers { get; set; }
        }

        public class UpdateReportRsp : RSPBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            public int ID { get; set; }
        }

        public class ReportAnswerD
        {
            /// <summary>
            /// 問題ID
            /// </summary>
            [Required]
            public string QuestionID { get; set; }
            /// <summary>
            /// 答案
            /// </summary>
            [MaxLength(3000)]
            public string Value { get; set; }
        }
    }
}
