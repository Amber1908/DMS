using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// AI 模組資料
    /// </summary>
    public class X1AI
    {
        /// <summary>
        /// 商品編號
        /// </summary>
        public string pcode { get; set; }

        /// <summary>
        /// AI 模組名稱
        /// </summary>
        public string ai_name { get; set; }

        /// <summary>
        /// AI 模組描述
        /// </summary>
        public string ai_desc { get; set; }

        /// <summary>
        /// AI 模組類型
        /// </summary>
        public string ai_type { get; set; }

        /// <summary>
        /// AI 掃描所需點數
        /// </summary>
        public int ai_value { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// 狀態 1:正常 0:註銷
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