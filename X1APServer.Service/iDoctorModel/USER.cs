using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// 會員基本資料
    /// </summary>
    public class USER
    {
        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// EMAIL驗證
        /// </summary>
        public int emailcheck { get; set; }

        /// <summary>
        /// 密碼(SHA256)
        /// </summary>
        public string pswd { get; set; }

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string displayname { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string firstname { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string lastname { get; set; }

        /// <summary>
        /// 國別代碼
        /// </summary>
        public string country_code { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? birthday { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string addr { get; set; }

        /// <summary>
        /// 最後變更密碼日期
        /// </summary>
        public DateTime changepwsdtime { get; set; }

        /// <summary>
        /// 狀態 1:正常 0:註銷
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string memo { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime createtime { get; set; }
    }
}