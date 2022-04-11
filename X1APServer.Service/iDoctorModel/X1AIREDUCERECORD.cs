using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// X1 掃描扣點紀錄
    /// </summary>
    public class X1AIREDUCERECORD
    {
        /// <summary>
        /// SN
        /// </summary>
        public int sn { get; set; }

        /// <summary>
        /// 用戶帳號 email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 群組序號
        /// </summary>
        public int group_sn { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        public string pcode { get; set; }

        /// <summary>
        /// AI 掃描所需點數
        /// </summary>
        public int ai_value { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime createtime { get; set; }
    }
}