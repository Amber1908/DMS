using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;

namespace X1APServer.Service.Model
{
    public class GetAllGroupM
    {
        public class GetAllGroupReq : REQBase
        {

        }

        public class GetAllGroupRsp : RSPBase
        {
            /// <summary>
            /// 群組清單
            /// </summary>
            public List<X1_PatientGroup> Data { get; set; }
        }
    }
}
