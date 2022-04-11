using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddReportAuthM
    {
        public class AddReportAuthReq : REQBase
        {
            /// <summary>
            /// Report ID
            /// </summary>
            public int ReportID { get; set; }
            /// <summary>
            /// 權限列表
            /// </summary>
            public List<AuthItem> AuthList { get; set; }
        }

        public class AddReportAuthRsp : RSPBase
        {

        }

        public class AuthItem
        {
            /// <summary>
            /// 權限項目名稱
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 權限值
            /// </summary>
            public string Value { get; set; }
        }
    }
}
