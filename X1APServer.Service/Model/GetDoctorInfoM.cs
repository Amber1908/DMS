using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetDoctorInfoM
    {
        public class GetDoctorInfoReq : REQBase
        {
            /// <summary>
            /// 醫生 ID
            /// </summary>
            [Required]
            public Nullable<int> ID { get; set; }
        }

        public class GetDoctorInfoRsp : RSPBase
        {
            /// <summary>
            /// 醫生 ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 醫生姓名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 醫生電話
            /// </summary>
            public string Tel { get; set; }
            /// <summary>
            /// 醫生所屬醫院
            /// </summary>
            public string Hospital { get; set; }
            /// <summary>
            /// 醫生性別
            /// </summary>
            public int Gender { get; set; }
        }
    }
}
