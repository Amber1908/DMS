using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetICD10M
    {
        public class GetICD10Req : REQBase
        {
            /// <summary>
            /// icd10關鍵字
            /// </summary>
            [Required]
            public string icd10code { get; set; }
        }

        public class GetICD10Rsp : RSPBase
        {
            /// <summary>
            /// 搜尋到的ICD10列表
            /// </summary>
            public List<ICD10> ICD10List { get; set; }
        }

        public class ICD10
        {
            public string FullCode { get; set; }
            //public string CategoryCode { get; set; }
            //public string DiagnosisCode { get; set; }
            public string AbbreviatedDescription { get; set; }
            //public string FullDescription { get; set; }
            //public string CategoryTitle { get; set; }
        }
    }
}
