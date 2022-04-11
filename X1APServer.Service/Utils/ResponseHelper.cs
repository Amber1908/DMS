using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Model;

namespace X1APServer.Service.Utils
{
    public static class ResponseHelper
    {
        public static RSPBase CreateResponse(ErrorCode errorCode, string message)
        {
            var rsp = new RSPBase()
            {
                ReturnCode = errorCode,
                ReturnMsg = message
            };
            return rsp;
        }

        public static RSPBase Ok()
        {
            return CreateResponse(ErrorCode.OK, "ok");
        }
    }
}
