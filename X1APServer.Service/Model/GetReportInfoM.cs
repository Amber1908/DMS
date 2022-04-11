using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetReportInfoM
    {
        public class GetReportInfoReq : REQBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            [Required]
            public int ReportID { get; set; }
        }

        public class GetReportInfoRsp : RSPBase
        {
            /// <summary>
            /// 醫令
            /// </summary>
            public Order Order { get; set; }
            /// <summary>
            /// 檢體
            /// </summary>
            public Specimen Specimen { get; set; }
            /// <summary>
            /// 診斷
            /// </summary>
            public DiagnosisRecord DiagnosisRecord { get; set; }
            /// <summary>
            /// 醫生資料
            /// </summary>
            public GetDoctorInfoListM.DoctorInfo DoctorInfo { get; set; }
            /// <summary>
            /// 匯出報告時間
            /// </summary>
            public DateTime? ReportDate { get; set; }
        }

        public class Order
        {
            /// <summary>
            /// 醫令
            /// </summary>
            public string OrderDetail { get; set; }
            /// <summary>
            /// 狀態
            /// </summary>
            public string State { get; set; }
        }

        public class Specimen
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 檢體類別
            /// </summary>
            public string SpecimenCategory { get; set; }
            /// <summary>
            /// 採檢日期
            /// </summary>
            public DateTime InspectionDate { get; set; }
            /// <summary>
            /// 收件日期
            /// </summary>
            public DateTime RecvDate { get; set; }
            /// <summary>
            /// 癌醫檢體編號
            /// </summary>
            public string CCSpecimenID { get; set; }
            /// <summary>
            /// 台成檢體編號
            /// </summary>
            public string TCTCSpecimenID { get; set; }
            /// <summary>
            /// 骨髓抽取位置
            /// </summary>
            public string Site { get; set; }
            /// <summary>
            /// 骨髓抽取位置其他
            /// </summary>
            public string SiteOthers { get; set; }
            public string Anticoagulate { get; set; }
        }

        public class DiagnosisRecord
        {
            /// <summary>
            /// 送檢醫院
            /// </summary>
            public string MorphFrom { get; set; }
            /// <summary>
            /// 科別
            /// </summary>
            public string Division { get; set; }
            /// <summary>
            /// 送檢醫師
            /// </summary>
            public string Doctor { get; set; }
            /// <summary>
            /// 開單日期
            /// </summary>
            public DateTime DiagnosisDate { get; set; }
            /// <summary>
            /// 診斷碼
            /// </summary>
            public string DiagnosisNo { get; set; }
            /// <summary>
            /// 狀態碼
            /// </summary>
            public string State { get; set; }
        }
    }
}
