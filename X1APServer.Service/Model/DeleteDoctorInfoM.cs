using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class DeleteDoctorInfoM
    {
        public class DeleteDoctorInfoReq : REQBase
        {
            /// <summary>
            /// 醫生 ID
            /// </summary>
            public int ID { get; set; }
        }

        public class DeleteDoctorInfoRsp : RSPBase
        {

        }
    }
}
