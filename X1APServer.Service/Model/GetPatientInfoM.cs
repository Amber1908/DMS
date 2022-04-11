using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetPatientInfoM
    {
        public class GetPatientInfoReq : REQBase
        {
            /// <summary>
            /// 個案ID
            /// </summary>
            [Required]
            public int ID { get; set; }
        }

        public class GetPatientInfoRsp : RSPBase
        {
            /// <summary>
            /// 個案資料
            /// </summary>
            public PatientInfo Patient { get; set; }
        }

        public class PatientInfo
        {
            /// <summary>
            /// 個案ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 個案代碼
            /// </summary>
            public string PUID { get; set; }
            /// <summary>
            /// 病歷號
            /// </summary>
            public string IDNo { get; set; }
            /// <summary>
            /// 國籍 1: 本國, 2: 外籍人士
            /// </summary>
            public string PUCountry { get; set; }
            /// <summary>
            /// 個案姓名
            /// </summary>
            public string PUName { get; set; }
            /// <summary>
            /// 個案生日
            /// </summary>
            public System.DateTime PUDOB { get; set; }
            /// <summary>
            /// 個案性別
            /// </summary>
            public string Gender { get; set; }
            /// <summary>
            /// 聯絡人電話
            /// </summary>
            public string ContactPhone { get; set; }
            /// <summary>
            /// 聯絡人關係
            /// </summary>
            public string ContactRelation { get; set; }
            /// <summary>
            /// 連絡電話
            /// </summary>
            public string Phone { get; set; }
            /// <summary>
            /// 教育程度
            /// </summary>
            public string Education { get; set; }
            /// <summary>
            /// 地址地區碼
            /// </summary>
            public string AddrCode { get; set; }
            /// <summary>
            /// 所屬衛生所醫療機構代碼
            /// </summary>
            public string HCCode { get; set; }
            /// <summary>
            /// 居住地址
            /// </summary>
            public string Addr { get; set; }
            /// <summary>
            /// 戶籍地區碼
            /// </summary>
            public string Domicile { get; set; }
        }
    }
}
