using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetPinQuestM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 病患ID
            /// </summary>
            [Required]
            public int PatientID { get; set; }
            /// <summary>
            /// 取得前N項紀錄
            /// </summary>
            public int Amount { get; set; } = 5;
        }

        public class Response : RSPBase
        {
            public List<QuestionGroup> Data { get; set; }
        }

        public class QuestionGroup
        {
            /// <summary>
            /// 問題標題
            /// </summary>
            public string GroupTitle { get; set; }
            /// <summary>
            /// 紀錄清單
            /// </summary>
            public List<Record> RecordList { get; set; }
        }

        public class Record
        {
            /// <summary>
            /// 填寫日期
            /// </summary>
            public DateTime Date { get; set; }
            /// <summary>
            /// 問題題目
            /// </summary>
            public string QuestionTitle { get; set; }
            /// <summary>
            /// 填寫答案
            /// </summary>
            public string Value { get; set; }
            /// <summary>
            /// 色碼
            /// </summary>
            public string Color { get; set; }
            /// <summary>
            /// 是否為標準值
            /// </summary>
            public bool Normal { get; set; }
        }
    }
}
