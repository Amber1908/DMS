using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class ExportReportM
    {
        public class ExportReportReq : REQBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            [Required]
            public int? ID { get; set; }
            /// <summary>
            /// 病患ID
            /// </summary>
            /// 
            [Required]
            public int? PatientID { get; set; }
            /// <summary>
            /// 抓取指定日期前最新的報告
            /// </summary>
            public DateTime? SpecificDate { get; set; } = DateTime.Now;
            /// <summary>
            /// 額外資訊
            /// </summary>
            public List<KeyValue> Extra { get; set; }
        }

        public class ExportReportRsp : RSPBase
        {
            /// <summary>
            /// PDF 下載URL
            /// </summary>
            public string pdfURL { get; set; }
        }

        public class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
