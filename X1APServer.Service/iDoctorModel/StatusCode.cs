using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.iDoctorModel
{
    public static class StatusCode
    {
        public static readonly string SUCCESS = "0000";
        public static readonly string ERROR_1 = "0001";
        public static readonly string ERROR_2 = "0002";
        public static readonly string FAIL_1 = "1001";
        public static readonly string FAIL_2 = "1002";
        public static readonly string FAIL_3 = "1003";
        public static readonly string EXCEPTION = "2001";
        public static readonly string WARNING = "3001";
    }
}
