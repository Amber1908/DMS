using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetReportTemplateM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// Template ID
            /// </summary>
            public int ID { get; set; }
        }

        public class Response : RSPBase
        {
            /// <summary>
            /// 報告模板資料
            /// </summary>
            public ReportTemplate Data { get; set; }
        }

        public class ReportTemplate
        {
            /// <summary>
            /// Template ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 檔案Url
            /// </summary>
            public string FileUrl { get; set; }
            /// <summary>
            /// 建立日期
            /// </summary>
            public System.DateTime CreateDate { get; set; }
            /// <summary>
            /// 建立人員
            /// </summary>
            public string CreateMan { get; set; }
            /// <summary>
            /// 修改日期
            /// </summary>
            public System.DateTime ModifyDate { get; set; }
            /// <summary>
            /// 修改人員
            /// </summary>
            public string ModifyMan { get; set; }
            /// <summary>
            /// 模板名稱
            /// </summary>
            public string Name { get; set; }
        }
    }
}
