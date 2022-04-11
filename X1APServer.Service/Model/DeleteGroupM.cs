using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class DeleteGroupM
    {
        public class DeleteGroupReq : REQBase
        {
            /// <summary>
            /// 要刪除的群組 ID
            /// </summary>
            public int ID { get; set; }
        }

        public class DeleteGroupRsp : RSPBase
        {

        }
    }
}
