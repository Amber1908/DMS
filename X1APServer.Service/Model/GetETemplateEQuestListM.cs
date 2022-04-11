using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;

namespace X1APServer.Service.Model
{
    public class GetETemplateEQuestListM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 匯出模板ID
            /// </summary>
            /// 
            [Required]
            public int ExportTemplateID { get; set; }
        }

        public class Response : RSPBase
        {
            /// <summary>
            /// 額外問題清單
            /// </summary>
            public List<TemplateExtraQuest> ExtraQuestList { get; set; }
        }

        public class TemplateExtraQuest
        {
            public int ID { get; set; }
            public int ExportTemplateID { get; set; }
            public string QuestName { get; set; }
            public string QuestText { get; set; }
            public string QuestType { get; set; }
            public bool Required { get; set; }
        }
    }
}
