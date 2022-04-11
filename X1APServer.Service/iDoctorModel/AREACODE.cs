using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// 地區代碼表
    /// </summary>
    public class AREACODE
    {
        /// <summary>
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 地區名稱
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 地區城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 地區鄉鎮
        /// </summary>
        public string Town { get; set; }
    }
}