using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetBasePathM
    {
        public class GetBasePathReq : REQBase
        {

        }

        public class GetBasePathRsp : RSPBase
        {
            // 圖片資料夾路徑
            public string ImageFolderPath { get; set; }
            // 工作目錄資料夾路徑
            public string WorkingFolderPath { get; set; }
        }
    }
}
