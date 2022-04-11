using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.DataAnnotaionExt;

namespace X1APServer.Service.Model
{
    public class X1UserDataSaveM
    {
        public class X1UserDataSaveReq : REQBase
        {
            /// <summary>
            /// 主表ID
            /// </summary>
            [Required]
            public int MainID { get; set; }
            /// <summary>
            /// 使用者姓名
            /// </summary>
            [Required]
            [MaxLength(30)]
            public string AccName { get; set; }

            /// <summary>
            /// 擔當角色(I:醫檢師)
            /// </summary>
            public string RoleType { get; } = "I";

            /// <summary>
            /// 處理狀態(N: 處理中, Y: 結案)
            /// </summary>
            [MaxLength(1)]
            public string UState { get; set; } = "N";

            /// <summary>
            /// 檔案絕對路徑
            /// </summary>
            public string ArchFilePath { get; set; }

            /// <summary>
            /// 檔案名稱
            /// </summary>
            [RegularExpression("^[^\\\\\\/]+$", ErrorMessage = "{0}不得包含斜線")]
            [MaxFileName(50)]
            public string ArchFileName { get; set; }

            /// <summary>
            /// 檔案副檔名名稱
            /// </summary>
            [RegularExpression("^[^\\\\\\/]+$", ErrorMessage = "{0}不得包含斜線")]
            [MaxLength(7)]
            public string ArchFileExtName { get; set; }

            /// <summary>
            /// 結案類別
            /// </summary>
            [Required]
            public Type Type { get; set; }
        }
        public class X1UserDataSaveRsp : RSPBase
        {
        }

        /// <summary>
        /// 結案類別
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Report 類別
            /// </summary>
            Report = 1,
            /// <summary>
            /// AI 類別
            /// </summary>
            AI = 2
        }
    }
}
