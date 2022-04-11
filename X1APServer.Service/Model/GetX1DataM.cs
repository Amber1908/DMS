using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetX1DataM
    {
        public class Request
        {
            /// <summary>
            /// 檢驗單編號 ID
            /// </summary>
            [Required]
            public int ID { get; set; }
        }

        public class Response : RSPBase
        {
            /// <summary>
            /// X1資料 string
            /// </summary>
            public string X1Data { get; set; }
        }
    }
}
