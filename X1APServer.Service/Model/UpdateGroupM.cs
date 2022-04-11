using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateGroupM
    {
        public class UpdateGroupReq : REQBase
        {
            /// <summary>
            /// 群組清單
            /// </summary>
            public List<PatientGroup> PatientGroups { get; set; }
        }

        public class UpdateGroupRsp : RSPBase
        {

        }

        public class PatientGroup
        {
            /// <summary>
            /// 群組 ID
            /// </summary>
            public int? ID { get; set; }
            /// <summary>
            /// 群組名稱
            /// </summary>
            public string GroupName { get; set; }
            /// <summary>
            /// 群組狀態
            /// </summary>
            public PatientGroupState State { get; set; }
        }

        public enum PatientGroupState {
            /// <summary>
            /// 無變更
            /// </summary>
            UnChange = 0,
            /// <summary>
            /// 新增
            /// </summary>
            New = 1,
            /// <summary>
            /// 已變更
            /// </summary>
            Modify = 2,
            /// <summary>
            /// 刪除
            /// </summary>
            Delete = 3
        }
    }
}
