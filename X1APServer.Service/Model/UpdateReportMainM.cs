using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateReportMainM
    {
        public class UpdateReportMainReq : REQBase
        {
            /// <summary>
            /// 是否刪除
            /// </summary>
            public bool? IsDelete { get; set; }
            /// <summary>
            /// 問卷結構
            /// </summary>
            [Required]
            public AddReportMainM.QuestionnaireStructure Structure { get; set; }
            private DateTime? reserveDate;
            /// <summary>
            /// 預定發佈時間
            /// </summary>
            public DateTime? ReserveDate
            {
                get
                {
                    return reserveDate;
                }
                set
                {
                    if (!value.HasValue)
                        value = DateTime.Now;

                    reserveDate = value;
                }
            }
        }

        public class UpdateReportMainRsp : RSPBase
        {
            /// <summary>
            /// 問卷結構
            /// </summary>
            public AddReportMainM.QuestionnaireStructure Structure { get; set; }
        }

        public class QuestionnaireStructure
        {
            [Required]
            public int ID { get; set; }
            public string Title { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public List<AddReportMainM.Group> Children { get; set; }
            public int IndexNum { get; set; }
        }
    }
}
