using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.DataAnnotaionExt;

namespace X1APServer.Service.Model
{
    public class GetDoctorInfoListM
    {
        public class GetDoctorInfoListReq : REQBase
        {
            /// <summary>
            /// 頁數
            /// </summary>
            [Required]
            [MinValue(0)]
            public int Page { get; set; }
            /// <summary>
            /// 每頁筆數
            /// </summary>
            [MinValue(0)]
            public int RowInPage { get; set; } = 30;
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
        }

        public class GetDoctorInfoListRsp : RSPBase
        {
            /// <summary>
            /// 醫生清單
            /// </summary>
            public List<DoctorInfo> DoctorInfoList { get; set; }
        }

        public class DoctorInfo
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
