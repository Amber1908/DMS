using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// 會員登入資料暫存物件
    /// </summary>
    public class PUSID
    {
        /// <summary>
        /// 鍵值
        /// </summary>
        public string pusid { get; set; }

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
        /// LOGO
        /// </summary>
        public int logo { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// sessionkey
        /// </summary>
        public string sessionkey { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime createtime { get; set; }
    }
}