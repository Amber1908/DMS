using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class GetCervixFormM
    {
        public class GetCervixFormReq
        {
            /// <summary>
            /// 醫師/醫檢師 代碼
            /// 不篩檢時留空
            /// </summary>
            public string DoctorNo { get; set; }
            /// <summary>
            /// 表單狀態 1待檢驗、2檢驗中、3待覆核、4覆核中、5已覆核、6已結案、7已匯出
            /// 不篩檢時填 0
            /// </summary>
            public int Status { get; set; }
        }
        public class GetCervixFormRsp : RSPBase
        {
            /// <summary>
            /// 檢驗單列表
            /// </summary>
            public List<CervixForm> CervixFormList { get; set; }
        }

        public class CervixForm
        {
            /// <summary>
            /// 檢驗單編號
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 23．抹片細胞病理編號
            /// </summary>
            public string Vix_23 { get; set; }
            /// <summary>
            /// 23-1．採檢片數
            /// </summary>
            public string Vix_23_1 { get; set; }
            /// <summary>
            /// 23-2．抹片掃描檔案名稱
            /// </summary>
            public string Vix_23_2 { get; set; }
            /// <summary>
            /// 表單狀態 1待檢驗、2檢驗中、3待覆核、4覆核中、5已覆核、6已結案、7已匯出
            /// </summary>
            public int Status { get; set; }
            /// <summary>
            /// 醫檢師代碼
            /// </summary>
            public string DoctorNo1 { get; set; }
            /// <summary>
            /// 醫師代碼
            /// </summary>
            public string DoctorNo2 { get; set; }
            /// <summary>
            /// 醫檢師姓名
            /// </summary>
            public string DoctorName1 { get; set; }
            /// <summary>
            /// 醫師姓名
            /// </summary>
            public string DoctorName2 { get; set; }
        }
    }
}
