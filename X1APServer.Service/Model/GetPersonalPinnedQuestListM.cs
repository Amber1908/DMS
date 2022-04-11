using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetPersonalPinnedQuestListM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 病患ID
            /// </summary>
            public int PatientID { get; set; }
        }

        public class Response : RSPBase
        {
            /// <summary>
            /// 關注問題清單
            /// </summary>
            public List<GetPinnedQuestListM.PinnedQuest> Data { get; set; }
        }
    }
}
