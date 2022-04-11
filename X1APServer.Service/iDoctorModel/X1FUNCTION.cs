using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDoctorTools.Models
{
    /// <summary>
    /// X1 功能解鎖
    /// </summary>
    public class X1FUNCTION
    {
        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// CZI 開檔解鎖
        /// </summary>
        public bool CZI { get; set; }

        /// <summary>
        /// MRXS 開檔解鎖
        /// </summary>
        public bool MRXS { get; set; }

        /// <summary>
        /// DCM 開檔解鎖
        /// </summary>
        public bool DCM { get; set; }

        /// <summary>
        /// 存座標小圖解鎖
        /// </summary>
        public bool SAVEIMAGE { get; set; }

        public X1FUNCTION(string email)
        {
            this.email = email;
            CZI = false;
            MRXS = false;
            DCM = false;
            SAVEIMAGE = false;
        }
    }
}