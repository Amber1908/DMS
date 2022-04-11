using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdatePinnedQuestM
    {
        public class Request : REQBase
        {
            public List<PinnedQuest> PinnedQuestList { get; set; }
        }

        public class PinnedQuest
        {
            /// <summary>
            /// 關注ID
            /// </summary>
            public int PinnedID { get; set; }
            /// <summary>
            /// 關注名稱
            /// </summary>
            public string PinnedName { get; set; }
            /// <summary>
            /// 是否顯示
            /// </summary>
            public bool Visible { get; set; }
            /// <summary>
            /// 排序編號
            /// </summary>
            public int Index { get; set; }
        }

        public class Response : RSPBase
        {

        }
    }
}
