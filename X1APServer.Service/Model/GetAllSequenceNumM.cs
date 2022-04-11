using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetAllSequenceNumM
    {
        public class Request : REQBase
        {

        }

        public class Response : RSPBase
        {
            /// <summary>
            /// 批號清單
            /// </summary>
            public List<string> SequenceNumList { get; set; }
        }
    }
}
