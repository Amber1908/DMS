using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetReportMainFileM
    {
        public class GetReportMainFileReq : REQBase
        {

        }

        public class GetReportMainFileRsp : RSPBase
        {
            public int ID { get; set; }
            public int RQID { get; set; }
            public byte[] FileData { get; set; }
            public string FileName { get; set; }
            public string MimeType { get; set; }
            public int RMID { get; set; }
        }
    }
}
