using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class DeleteScheduleM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 要刪除的回診ID
            /// </summary>
            public int ID { get; set; }
        }

        public class Response : RSPBase
        {

        }
    }
}
