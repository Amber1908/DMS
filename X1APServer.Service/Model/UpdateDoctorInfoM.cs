using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateDoctorInfoM
    {
        public class UpdateDoctorInfoReq : REQBase
        {
            /// <summary>
            /// 醫生資料
            /// </summary>
            [Required]
            public DoctorInfo Data { get; set; }
        }

        public class UpdateDoctorInfoRsp : RSPBase
        {

        }

        public class DoctorInfo
        {
            /// <summary>
            /// 醫生 ID
            /// </summary>
            [Required]
            public Nullable<int> ID { get; set; }
            /// <summary>
            /// 醫生姓名
            /// </summary>
            [Required]
            public string Name { get; set; }
            /// <summary>
            /// 醫生電話
            /// </summary>
            [Required]
            public string Tel { get; set; }
            /// <summary>
            /// 醫生所屬醫院
            /// </summary>
            [Required]
            public string Hospital { get; set; }
            /// <summary>
            /// 醫生性別
            /// </summary>
            public Nullable<int> Gender { get; set; }
        }
    }
}
