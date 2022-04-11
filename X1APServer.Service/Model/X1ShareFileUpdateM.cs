using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1ShareFileUpdateM
    {
        public class X1ShareFileUpdateReq : REQBase
        {
            /// <summary>
            /// 使用者姓名
            /// </summary>
            public string AccName { get; set; }
            /// <summary>
            /// 辨識檔案ID Ex: 1 : 類別檔
            /// </summary>
            public int LogKey { get; set; }
            /// <summary>
            /// 更新檔位置
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 更新檔案名稱
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 更新檔案副檔名
            /// </summary>
            public string FileExtName { get; set; }
        }

        public class X1ShareFileUpdateRsp : RSPBase
        {
            
        }
        
    }
}
