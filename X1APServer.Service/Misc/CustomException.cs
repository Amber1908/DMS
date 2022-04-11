using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Model;

namespace X1APServer.Service.Misc
{
    public class CustomException : Exception
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
    }
}
