using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetQuestionListM
    {
        public class GetQuestionListReq : REQBase
        {
            /// <summary>
            /// 問題文字列表
            /// </summary>
            public IEnumerable<string> QuestTextList { get; set; }
        }

        public class GetQuestionListRsp : RSPBase
        {
            /// <summary>
            /// 問題列表
            /// </summary>
            public IEnumerable<Question> QuestionList { get; set; }
        }

        public class Question
        {
            /// <summary>
            /// 問題 ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 問題類型
            /// </summary>
            public int QuestionType { get; set; }
            /// <summary>
            ///  主Report ID
            /// </summary>
            public int ReportID { get; set; }
            /// <summary>
            /// 問題文字
            /// </summary>
            public string QuestionText { get; set; }
            /// <summary>
            /// 問題描述
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// 是否必填
            /// </summary>
            public bool Required { get; set; }
            /// <summary>
            /// CodingBook 標題
            /// </summary>
            public string CodingBookTitle { get; set; }
            /// <summary>
            /// CodingBook 索引
            /// </summary>
            public int CodingBookIndex { get; set; }
            /// <summary>
            /// 答案選項
            /// </summary>
            public string AnswerOption { get; set; }
        }
    }
}
