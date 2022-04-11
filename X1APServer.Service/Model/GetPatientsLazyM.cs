using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.DataAnnotaionExt;

namespace X1APServer.Service.Model
{
    public class GetPatientsLazyM
    {
        public class GetPatientLazyReq : REQBase
        {
            /// <summary>
            /// 頁數
            /// </summary>
            [MinValue(0)]
            public int? Page { get; set; }
            /// <summary>
            /// 每頁筆數
            /// </summary>
            [MinValue(0)]
            public int? RowInPage { get; set; } = 30;
            /// <summary>
            /// 要搜尋的個案編號(ex. 身分證字號)
            /// </summary>
            //public string PUID { get; set; }
            /// <summary>
            /// 要搜尋的姓名
            /// </summary>
            public string PUName { get; set; }
            /// <summary>
            /// 要搜尋的身份證號
            /// </summary>
            public string IDNo { get; set; }
            ///// <summary>
            ///// 排序問題清單
            ///// </summary>
            //public List<SortQuest> SortQuestList { get; set; }
            ///// <summary>
            ///// 需要的報告問題ID
            ///// </summary>
            //public List<string> QuestNoList { get; set; }
        }

        public class GetPatientLazyRsp : RSPBase
        {
            /// <summary>
            /// 個案清單
            /// </summary>
            public IEnumerable<PatientInfo> Patients { get; set; }
            /// <summary>
            /// 個案總數
            /// </summary>
            public int TotalPatient { get; set; }
        }

        public class PatientInfo
        {
            /// <summary>
            /// 個案ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 個案代碼
            /// </summary>
            public string PUID { get; set; }
            /// <summary>
            /// 病歷號
            /// </summary>
            public string IDNo { get; set; }
            /// <summary>
            /// 個案姓名
            /// </summary>
            public string PUName { get; set; }
            /// <summary>
            /// 個案生日
            /// </summary>
            public System.DateTime PUDOB { get; set; }
            /// <summary>
            /// 個案性別
            /// </summary>
            public string Gender { get; set; }
            /// <summary>
            /// 下次回診時間
            /// </summary>
            public DateTime? NextVisitTime { get; set; }
            ///// <summary>
            ///// 答案清單
            ///// </summary>
            //public List<QuestAns> QuestAnsList { get; set; }
        }

        public class QuestAns
        {
            /// <summary>
            /// 問題編號
            /// </summary>
            public string QuestNo { get; set; }
            /// <summary>
            /// 問題答案
            /// </summary>
            public string Answer { get; set; }
        }

        public class SortQuest
        {
            /// <summary>
            /// 問題編號
            /// </summary>
            public string QuestNo { get; set; }
            /// <summary>
            /// 排序規則(asc(預設): 由小到大, desc: 由大到小)
            /// </summary>
            public string SortType { get; set; }
        }
    }
}
