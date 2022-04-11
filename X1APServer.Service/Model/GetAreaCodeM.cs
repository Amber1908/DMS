using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDoctorTools.Models;

namespace X1APServer.Service.Model
{
    public class GetAreaCodeM
    {
        public class Request
        {
        }

        public class Response : RSPBase
        {
            public List<AREACODE> Data { get; set; }
        }
    }
}
