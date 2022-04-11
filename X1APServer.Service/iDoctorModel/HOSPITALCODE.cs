using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// 醫事機構代碼
    /// </summary>
    public class HOSPITALCODE
    {
        /// <summary>
        /// 分區別
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 醫事機構代碼
        /// </summary>
        public string HospitalCode { get; set; }
        /// <summary>
        /// 醫事機構名稱
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 機構地址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 電話區域號碼
        /// </summary>
        public string PhoneArea { get; set; }
        /// <summary>
        /// 電話號碼
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 特約類別
        /// </summary>
        public string SpKind { get; set; }
        /// <summary>
        /// 型態別
        /// </summary>
        public string HospitalType { get; set; }
        /// <summary>
        /// 醫事機構種類
        /// </summary>
        public string HospitalKind { get; set; }
        /// <summary>
        /// 終止合約或歇業日期
        /// </summary>
        public string StopTime { get; set; }
        /// <summary>
        /// 開業狀況
        /// </summary>
        public string Opening { get; set; }
    }
}