using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetFileM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 要取得的檔案ID
            /// </summary>
            public Guid ID { get; set; }
        }

        public class Response : RSPBase
        {
            /// <summary>
            /// 檔案路徑
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 檔案名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// MimeType
            /// </summary>
            public string MimeType { get; set; }
        }
    }
}
