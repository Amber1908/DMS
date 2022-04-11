using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateSpecimenM
    {
        public class UpdateSpecimenReq : REQBase
        {
            /// <summary>
            /// 要更新的檢體ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 檢體類別
            /// </summary>
            [MaxLength(100)]
            public string SpecimenCategory { get; set; }
            /// <summary>
            /// 採檢日期
            /// </summary>
            public DateTime? InspectionDate { get; set; }
            /// <summary>
            /// 收件日期
            /// </summary>
            public DateTime? RecvDate { get; set; }
            [MaxLength(100)]
            public string Site { get; set; }
            [MaxLength(60)]
            public string SiteOthers { get; set; }
            [MaxLength(100)]
            public string Anticoagulate { get; set; }
            /// <summary>
            /// 癌醫檢體編號
            /// </summary>
            [MaxLength(100)]
            public string CCSpecimenID { get; set; }
            /// <summary>
            /// 台成檢體編號
            /// </summary>
            [MaxLength(100)]
            public string TCTCSpecimenID { get; set; }
        }

        public class UpdateSpecimenRsp : RSPBase
        {

        }
    }
}
