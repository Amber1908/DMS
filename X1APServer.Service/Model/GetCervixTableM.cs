using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetCervixTableM
    {
        public class GetCervixTableReq
        {
            /// <summary>
            /// 表單編號， 帶 0 査全部
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 個案ID，帶 0 査全部
            /// </summary>
            public int CaseID { get; set; }
        }

        public class GetCervixTableRsp : RSPBase
        {
            public List<CervixTable> cervixTables { get; set; }
        }
    }
}
