using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class DeleteFileM
    {
        public class Reqeust : REQBase
        {
            /// <summary>
            /// 要刪除的檔案ID
            /// </summary>
            public Guid ID { get; set; }
        }

        public class Response : RSPBase
        {

        }
    }
}
