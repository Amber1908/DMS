using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddPatientM
    {
        public class AddPatientReq : REQBase
        {
            /// <summary>
            /// 身分證、護照等
            /// </summary>
            //[Required]
            //[MaxLength(10)]
            //public string PUID { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            //[Required]
            [MaxLength(100)]
            public string PUName { get; set; }
            /// <summary>
            /// 生日
            /// </summary>
            //[Required]
            public DateTime? PUDOB { get; set; }
            /// <summary>
            /// 性別(M: 男, F: 女)
            /// </summary>
            //[Required]
            [RegularExpression("^[MF]$", ErrorMessage = "{0} 只能是 M: 男性, F: 女性")]
            public string Gender { get; set; }
            /// <summary>
            /// 身份證號
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string IDNo { get; set; }
            /// <summary>
            /// 診斷
            /// </summary>
            [Required]
            public DiagnosisRecord DiagnosisRecord { get; set; }
            /// <summary>
            /// 聯絡人電話
            /// </summary>
            public string ContactPhone { get; set; }
        }

        public class AddPatientRsp : RSPBase
        {
            /// <summary>
            /// 病患ID
            /// </summary>
            public int PID { get; set; }
            /// <summary>
            /// 診斷ID清單
            /// </summary>
            public List<int> DIDList { get; set; }
            /// <summary>
            /// 醫令ID清單
            /// </summary>
            public List<int> OIDList { get; set; }
            /// <summary>
            /// 檢體ID清單
            /// </summary>
            public List<int> SIDList { get; set; }
        }

        public class DiagnosisRecord
        {
            /// <summary>
            /// 送檢醫院
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string MorphFrom { get; set; }
            /// <summary>
            /// 科別
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string Division { get; set; }
            /// <summary>
            /// 送檢醫師
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string Doctor { get; set; }
            /// <summary>
            /// 開單日期
            /// </summary>
            [Required]
            public System.DateTime DiagnosisDate { get; set; }
            /// <summary>
            /// 診斷碼
            /// </summary>
            [Required]
            [MaxLength(50)]
            public string DiagnosisNo { get; set; }
            /// <summary>
            /// 狀態碼(N: 新判, T: 追蹤, R:復發)
            /// </summary>
            [Required]
            [RegularExpression("^[NTR]$", ErrorMessage = "{0} 只能是 N: 新判, T: 追蹤, R: 復發")]
            public string State { get; set; }
            /// <summary>
            /// 醫令清單
            /// </summary>
            [Required]
            public List<Order> OrderList { get; set; }
        }

        public class Order
        {
            /// <summary>
            /// 醫令
            /// </summary>
            [Required]
            [MaxLength(500)]
            public string OrderDetail { get; set; }
            /// <summary>
            /// 狀態
            /// </summary>
            //[Required]
            [MaxLength(1)]
            public string State { get; set; }
            /// <summary>
            /// 癌醫檢體編號
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string CCSpecimenID { get; set; }
            /// <summary>
            /// 檢體
            /// </summary>
            public Specimen Specimen { get; set; }
        }

        public class Specimen
        {
            /// <summary>
            /// 檢體類別
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string SpecimenCategory { get; set; }
            /// <summary>
            /// 採檢日期
            /// </summary>
            [Required]
            public System.DateTime InspectionDate { get; set; }
            /// <summary>
            /// 收件日期
            /// </summary>
            //[Required]
            //public System.DateTime RecvDate { get; set; }
        }
    }
}
