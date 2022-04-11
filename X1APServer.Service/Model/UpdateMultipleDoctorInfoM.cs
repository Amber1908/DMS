using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateMultipleDoctorInfoM
    {
        public class UpdateMultipleDoctorInfoReq : REQBase
        {
            /// <summary>
            /// 要更新的醫生清單
            /// </summary>
            public List<UpdateDoctorInfoM.DoctorInfo> DoctorInfoList { get; set; }
        }

        public class UpdateMultipleDoctorInfoRsp : RSPBase
        {

        }
    }
}
