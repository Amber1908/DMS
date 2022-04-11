using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetTextTemplateM
    {
        public class GetTextTemplateReq : REQBase
        {
            /// <summary>
            /// 主 Question ID
            /// </summary>
            public int? QuestionID { get; set; }
            /// <summary>
            /// TextTemplate Group
            /// </summary>
            public int? Group { get; set; } = 1;
        }

        public class GetTextTemplateRsp : RSPBase
        {
            /// <summary>
            /// 罐頭語列表
            /// </summary>
            public List<TextTemplate> TextTemplateList { get; set; }
        }

        public class TextTemplate
        {
            /// <summary>
            /// 罐頭語 ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 主 Question ID
            /// </summary>
            public string QuestionID { get; set; }
            /// <summary>
            /// 罐頭語顯示文字
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 罐頭語值
            /// </summary>
            public string Value { get; set; }
        }
    }
}
