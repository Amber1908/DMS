using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// user group data model.
    /// </summary>
    public class USERGROUP
    {
        /// <summary>
        /// 群組序號
        /// </summary>
        public int group_sn { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string group_name { get; set; }

        /// <summary>
        /// AI 掃描點數
        /// </summary>
        public int x1scan_count { get; set; }

        /// <summary>
        /// 群組擁有者
        /// </summary>
        public string group_owner { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime lastupdatetime { get; set; }

        /// <summary>
        /// 狀態 1: 正常 2: 停用
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string memo { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime createtime { get; set; }
    }
}