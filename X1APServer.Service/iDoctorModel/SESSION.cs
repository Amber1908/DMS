using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// Session
    /// </summary>
    public class SESSION
    {
        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 病程網站序號
        /// </summary>
        public int web_sn { get; set; }

        /// <summary>
        /// 登入鍵值
        /// </summary>
        public string sessionkey { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime createtime { get; set; }
    }
}