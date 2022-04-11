using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static X1APServer.Service.Model.GetTextTemplateM;

namespace X1APServer.Service.Model
{
    public class GetPanelTextTemplateM
    {
        public class GetPanelTextTemplateReq : REQBase
        {
            /// <summary>
            /// Panel 類別
            /// </summary>
            [Required]
            public int PanelID { get; set; }
            /// <summary>
            /// Question ID
            /// </summary>
            [Required]
            public int QuestionID { get; set; }
        }

        public class GetPanelTextTemplateRsp : RSPBase
        {
            /// <summary>
            /// 罐頭語列表
            /// </summary>
            public List<TextTemplate> TextTemplateList { get; set; }
        }
    }
}
