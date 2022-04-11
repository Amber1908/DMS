using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;

namespace X1APServer.Service.Model
{
    public class AddOrUpdateScheduleM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// ID
            /// </summary>
            public int? ID { get; set; }
            /// <summary>
            /// 病患ID
            /// </summary>
            [Required]
            public int PatientID { get; set; }
            /// <summary>
            /// 回診內容
            /// </summary>
            public string ContentText { get; set; }
            /// <summary>
            /// 回診時間
            /// </summary>
            [Required]
            public System.DateTime ReturnDate { get; set; }
        }

        public class Response : RSPBase
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
        }
    }
}
