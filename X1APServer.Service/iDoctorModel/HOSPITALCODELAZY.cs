using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// 醫事機構代碼(簡略版)
    /// </summary>
    public class HOSPITALCODELAZY
    {
        /// <summary>
        /// 醫事機構代碼
        /// </summary>
        public string HospitalCode { get; set; }
        /// <summary>
        /// 醫事機構名稱
        /// </summary>
        public string HospitalName { get; set; }
    }
}