using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BMDC.Models.Auth.GetFunctionListM;

namespace X1APServer.Service.Model
{
    public class WebGetFunctionListM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 篩選功能狀態
            /// </summary>
            public string FuncStatus { get; set; } = "Y";
        }

        public class Response : RSPBase
        {
            /// <summary>
            /// 功能清單
            /// </summary>
            public List<Function> FunctionList { get; set; }
        }
       
    }
}
