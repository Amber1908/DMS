using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDoctorTools.Models;

namespace X1APServer.Service.Model
{
    public class GetHospitalCodeM
    {
        public class Request
        {
            public string code { get; set; }
        }

        public class Response : RSPBase
        {
            public List<HOSPITALCODE> Data { get; set; }
        }
    }
}
