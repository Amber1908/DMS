using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetPanelDataM
    {
        public class GetPanelDataReq : REQBase
        {
            /// <summary>
            /// Panel ID
            /// </summary>
            [Required]
            public int PanelID { get; set; }
        }

        public class GetPanelDataRsp : RSPBase
        {
            /// <summary>
            /// Panel 名稱
            /// </summary>
            public string PanelName { get; set; }
            /// <summary>
            /// Panel Json
            /// </summary>
            public string IgCollection { get; set; }
        }
    }
}
