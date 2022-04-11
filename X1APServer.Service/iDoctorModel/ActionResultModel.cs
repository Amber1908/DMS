using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// Action result model.
    /// </summary>
    public class ActionResultModel
    {
        /// <summary>
        /// Statuscode : 0000 or Error code
        /// </summary>
        public string statuscode { get; set; }
        /// <summary>
        /// Severity : 可能值為 SUCCESS,FAIL,WARN,TIMEOUT,EXCEPTION
        /// </summary>
        public string severity { get; set; }
        /// <summary>
        /// StatusDesc : 錯誤詳細訊息或回應資料, 登入交易成功時回傳 Sessionkey
        /// </summary>
        public string statusdesc { get; set; }
    }
}