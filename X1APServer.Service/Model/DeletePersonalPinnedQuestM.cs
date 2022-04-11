using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class DeletePersonalPinnedQuestM
    {
        public class Request : REQBase
        {
            /// <summary>
            /// 病患ID
            /// </summary>
            [Required]
            public int PatientID { get; set; }
        }

        public class Response : RSPBase
        {

        }
    }
}
