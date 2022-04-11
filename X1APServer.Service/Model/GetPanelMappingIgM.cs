using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetPanelMappingIgM
    {
        public class GetPanelMappingIgReq : REQBase
        {
            /// <summary>
            /// Panel ID
            /// </summary>
            [Required]
            public int PanelID { get; set; }
        }

        public class GetPanelMappingIgRsp : RSPBase
        {
            /// <summary>
            /// Panel 抗體對應清單
            /// </summary>
            public IEnumerable<PanelIg> PanelIgList { get; set; }
        }

        public class PanelIg
        {
            /// <summary>
            /// 抗體名稱
            /// </summary>
            public string IgName { get; set; }
            /// <summary>
            /// 細胞類型
            /// </summary>
            public string CellType { get; set; }
        }
    }
}
