using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;

namespace X1APServer.Service.Model
{
    public class CheckQuestNo
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 檢查編號的問卷類別
            /// </summary>
            public string Category { get; set; }
            /// <summary>
            /// 問題編號
            /// </summary>
            [Required]
            public string QuestNo { get; set; }
        }

        public class Response : RSPBase
        {
            public bool IsDuplicated { get; set; }
            public string Title { get; set; }
        }
    }
}
