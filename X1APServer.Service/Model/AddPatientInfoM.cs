using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddPatientInfoM
    {
        public class AddPatientInfoReq : REQBase
        {
            /// <summary>
            /// 身分證/病歷號/護照
            /// </summary>
            //[Required]
            //public string PUID { get; set; }
            /// <summary>
            /// 國籍 1: 本國, 2: 外籍人士
            /// </summary>
            public string PUCountry { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            [Required]
            public string PUName { get; set; }
            /// <summary>
            /// 生日
            /// </summary>
            //[Required]
            public DateTime? PUDOB { get; set; }
            /// <summary>
            /// 身份證字號
            /// </summary>
            [Required]
            public string IDNo { get; set; }
            /// <summary>
            /// 性別
            /// </summary>
            //[Required]
            [RegularExpression("^[MF]$", ErrorMessage = "{0} 只能是 M: 男性, F: 女性")]
            public string Gender { get; set; }
            /// <summary>
            /// 身高
            /// </summary>
            public decimal Height { get; set; }
            /// <summary>
            /// 體重
            /// </summary>
            public decimal Weight { get; set; }
            /// <summary>
            /// 家中電話
            /// </summary>
            public string Phone { get; set; }
            /// <summary>
            /// 手機號碼
            /// </summary>
            public string Cellphone { get; set; }
            /// <summary>
            /// 聯絡人電話
            /// </summary>
            [MaxLength(50)]
            public string ContactPhone { get; set; }
            /// <summary>
            /// 聯絡人關係
            /// </summary>
            [MaxLength(100)]
            public string ContactRelation { get; set; }
            /// <summary>
            /// 教育程度
            /// </summary>
            [MaxLength(1)]
            public string Education { get; set; }
            /// <summary>
            /// 地址地區碼
            /// </summary>
            [MaxLength(10)]
            public string AddrCode { get; set; }
            /// <summary>
            /// 所屬衛生所醫療機構代碼
            /// </summary>
            [MaxLength(10)]
            public string HCCode { get; set; }
            /// <summary>
            /// 居住地址
            /// </summary>
            [MaxLength(100)]
            public string Addr { get; set; }
            /// <summary>
            /// 戶籍地區碼
            /// </summary>
            [MaxLength(10)]
            public string Domicile { get; set; }
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
        }

        public class AddPatientInfoRsp : RSPBase
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
        }
    }
}
