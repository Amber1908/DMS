using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.DataAnnotaionExt;

namespace X1APServer.Service.Model
{
    public class GetICD10LazyM
    {
        public class GetICD10LazyReq : REQBase
        {
            /// <summary>
            /// 頁數
            /// </summary>
            [MinValue(0)]
            public int Page { get; set; } = 0;
            /// <summary>
            /// 每頁筆數
            /// </summary>
            [MinValue(0)]
            public int RowInPage { get; set; } = 30;
            /// <summary>
            /// icd10關鍵字
            /// </summary>
            public string ICD10Code { get; set; }
        }

        public class GetICD10LazyRsp : RSPBase
        {
            /// <summary>
            /// ICD10 清單
            /// </summary>
            public List<GetICD10M.ICD10> ICD10List { get; set; }
        }
    }
}
