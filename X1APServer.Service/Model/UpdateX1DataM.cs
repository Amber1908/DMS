using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateX1DataM
    {
        public class UpdateX1DataReq
        {
            /// <summary>
            /// 檢驗單編號 ID
            /// </summary>
            [Required]
            public int ID { get; set; }
            /// <summary>
            /// X1資料 string
            /// </summary>
            public string X1Data { get; set; }
        }

        public class UpdateX1DataRsp : RSPBase
        {
        }
    }
}
