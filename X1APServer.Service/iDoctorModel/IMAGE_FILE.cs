using System;

namespace iDoctorTools.Models
{
    /// <summary>
    /// Image file buffer.
    /// </summary>
    public class IMAGE_FILE
    {
        /// <summary>
        /// 檔案序號 (上傳時不用帶)
        /// </summary>
        public int image_id { get; set; }

        /// <summary>
        /// 檔案內容 byte[] Base64 編碼
        /// </summary>
        public string image_bytes { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string memo { get; set; }

        /// <summary>
        /// 建立時間 (上傳時不用帶)
        /// </summary>
        public DateTime createtime { get; set; }
    }
}