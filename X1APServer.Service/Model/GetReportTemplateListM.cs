using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;

namespace X1APServer.Service.Model
{
    public class GetReportTemplateListM
    {
        public class GetReportTemplateListReq : REQBase
        {

        }

        public class GetReportTemplateListRsp : RSPBase
        {
            /// <summary>
            /// 檔名清單
            /// </summary>
            public List<ExportTemplate> TemplateList { get; set; }
        }
        
        public class ExportTemplate
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public System.Guid SystemFileID { get; set; }
            public System.DateTime CreateDate { get; set; }
            public string CreateMan { get; set; }
            public System.DateTime ModifyDate { get; set; }
            public string ModifyMan { get; set; }
            public Nullable<System.DateTime> DeleteDate { get; set; }
            public string DeleteMan { get; set; }
            public bool IsDelete { get; set; }
        }
    }
}
