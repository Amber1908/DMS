using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddMultipleDoctorInfoM
    {
        public class AddMultipleDoctorInfoReq : REQBase
        {
            /// <summary>
            /// 要新增的多個醫生
            /// </summary>
            public List<AddDoctorInfoM.DoctorInfo> Data { get; set; }
        }

        public class AddMulitpleDoctorInfoRsp : RSPBase
        {

        }
    }
}
