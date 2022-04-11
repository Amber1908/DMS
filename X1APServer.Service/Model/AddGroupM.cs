using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddGroupM
    {
        public class AddGroupReq : REQBase
        {
            /// <summary>
            /// 群組名稱
            /// </summary>
            public string[] GroupName { get; set; }
        }

        public class AddGroupRsp : RSPBase
        {
            /// <summary>
            /// 新增的群組 ID
            /// </summary>
            public int[] ID { get; set; }
        }
    }
}
