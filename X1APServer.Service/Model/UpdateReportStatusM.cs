using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateReportStatusM
    {
        public class UpdateReportStatusReq : REQBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            [Required]
            public int ID { get; set; }
            public int Status { get; set; }
        }

        public class UpdateReportStatusRsp : RSPBase
        {
        }
    }
}
