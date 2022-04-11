using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetAreaCodeLazyM
    {
        public class Request
        {
        }

        public class Response : RSPBase
        {
            public List<AREACODELAZY> Data { get; set; }
        }

        public class AREACODELAZY
        {
            /// <summary>
            /// text
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// value
            /// </summary>
            public string value { get; set; }
        }
    }
}
