using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetScheduleListM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 病患ID
            /// </summary>
            [Required]
            public int PatientID { get; set; }
            /// <summary>
            /// 忽略已過期回診時間
            /// </summary>
            public bool IgnoreExpiredDate { get; set; } = false;
        }

        public class Response : RSPBase
        {
            /// <summary>
            /// 回診清單
            /// </summary>
            public List<Schedule> ScheduleList { get; set; }
        }

        public class Schedule
        {
            public int ID { get; set; }
            public int PatientID { get; set; }
            public string ContentText { get; set; }
            public System.DateTime ReturnDate { get; set; }
        }
    }
}
