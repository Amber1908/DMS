using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateFileM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 檔案ID
            /// </summary>
            public Guid ID { get; set; }
            /// <summary>
            /// 檔案路徑
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 檔案名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// Mime Type
            /// </summary>
            public string MimeType { get; set; }
        }

        public class Response : RSPBase
        {

        }
    }
}
