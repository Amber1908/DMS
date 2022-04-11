using System;
using System.ComponentModel.DataAnnotations;

namespace X1APServer.Service.Model
{
    [Serializable]
    public class REQBase
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        [Required]
        public string AccID { get; set; }
        /// <summary>
        /// Security字串，後續驗證Token時使用
        /// </summary>
        [Required]
        public string UserSecurityInfo { get; set; }
        /// <summary>
        /// 檢核功能代碼(FN0001: X1標記, FN0002: Report匯出, FN0003: 差異檢核與匯出, FN0004: 抹片狀態變更)
        /// </summary>
        [Required]
        public string FuncCode { get; set; }
        /// <summary>
        /// 檢核權限代碼(細部權限劃分，目前只有用到 1)
        /// </summary>
        [Required]
        public int AuthCode { get; set; } = 1;
    }
}