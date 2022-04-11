using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class UpdateCervixStatusM
    {
        public class UpdateCervixStatusReq
        {
            /// <summary>
            /// 檢驗單編號
            /// </summary>
            [Required]
            public int ID { get; set; }
            /// <summary>
            /// 表單狀態更新 1待檢驗、2檢驗中、3待覆核、4覆核中、5已覆核、6已結案、7已匯出
            /// </summary>
            [Required]
            public int Status { get; set; }
            /// <summary>
            /// 醫檢師代碼
            /// </summary>
            public string DoctorNo1 { get; set; }
            /// <summary>
            /// 醫檢師姓名
            /// </summary>
            public string DoctorName1 { get; set; }
            /// <summary>
            /// 醫師代碼
            /// </summary>
            public string DoctorNo2 { get; set; }
            /// <summary>
            /// 醫師姓名
            /// </summary>
            public string DoctorName2 { get; set; }
        }

        public class UpdateCervixStatusRsp : RSPBase
        { }
    }
}
