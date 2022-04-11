using iDoctorTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetHealthWebByUserM
    {
        public class Request
        {
            public string Email { get; set; }
        }

        public class Response : RSPBase
        {
            public List<HEALTHWEB> Data { get; set; }
        }
    }
}
