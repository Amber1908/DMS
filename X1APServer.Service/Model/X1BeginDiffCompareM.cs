using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class X1BeginDiffCompareM
    {
        public class X1BeginDiffCompareReq : REQBase
        {
            /// <summary>
            /// 檢體ID
            /// </summary>
            [Required]
            public Nullable<int> MainID { get; set; }
        }

        public class X1BeginDiffCompareRsp : RSPBase
        {

        }
    }
}
