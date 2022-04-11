using iDoctorTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetHealthWebM
    {
        public class Request : REQBase
        {
            public int web_sn { get; set; }
        }

        public class Response : RSPBase
        {
            public HEALTHWEB Data { get; set; }
        }
    }
}
