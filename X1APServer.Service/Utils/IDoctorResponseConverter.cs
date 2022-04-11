using iDoctorTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Model;

namespace X1APServer.Service.Utils
{
    public class IDoctorResponseConverter
    {
        public static RSPBase Convert(ActionResultModel iDoctorResponse)
        {
            return new RSPBase()
            {
                ReturnCode = ConvertStatuscode(iDoctorResponse.severity),
                ReturnMsg = iDoctorResponse.statusdesc
            };
        }

        private static ErrorCode ConvertStatuscode(string serverity)
        {
            switch(serverity)
            {
                case "SUCCESS":
                case "WARN":
                    return ErrorCode.OK;
                case "FAIL":
                    return ErrorCode.OperateError;
                case "TIMEOUT":
                    return ErrorCode.Timeout;
                case "EXCEPTION":
                    return ErrorCode.Exception;
                default:
                    return ErrorCode.Exception;
            }
        }
    }
}
