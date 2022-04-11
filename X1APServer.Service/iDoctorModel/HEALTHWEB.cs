using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// healthweb data model.
    /// </summary>
    public class HEALTHWEB
    {
        /// <summary>
        /// 病程網站序號
        /// </summary>
        public int web_sn { get; set; }

        /// <summary>
        /// 病程網站名稱
        /// </summary>
        public string web_name { get; set; }

        /// <summary>
        /// 病程網站資料庫
        /// </summary>
        public string web_db { get; set; }

        /// <summary>
        /// 病程網站擁有者
        /// </summary>
        public string web_owner { get; set; }

        /// <summary>
        /// 病程網站人數上限
        /// </summary>
        public int web_count { get; set; }

        /// <summary>
        /// 病程網站到期日
        /// </summary>
        public DateTime web_expire { get; set; }

        /// <summary>
        /// 狀態 1: 正常 2: 停用
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// LOGO
        /// </summary>
        public int logo { get; set; }

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