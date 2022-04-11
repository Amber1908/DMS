using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddOrUpdateReportTemplateM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public int? ID { get; set; }
            /// <summary>
            /// 模板名稱
            /// </summary>
            public string TemplateName { get; set; }
            /// <summary>
            /// 檔案路徑
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 額外問題清單
            /// </summary>
            public List<ExtraQuest> ExtraQuestList { get; set; }
        }

        public class Response : RSPBase
        {

        }

        public class ExtraQuest
        {
            /// <summary>
            /// 問題對應名稱
            /// </summary>
            public string QuestName { get; set; }
            /// <summary>
            /// 問題題目
            /// </summary>
            public string QuestText { get; set; }
            /// <summary>
            /// 問題類別
            /// </summary>
            public string QuestType { get; set; }
            /// <summary>
            /// 是否必填
            /// </summary>
            public bool Required { get; set; }
        }
    }
}
