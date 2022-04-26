using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;

namespace X1APServer.Service.Model
{
    public class AnsWithPatient
    {
        /// <summary>
        /// 個案ID
        /// </summary>
        public int PID { get; set; }
        /// <summary>
        /// 國籍代號
        /// </summary>
        public int PUCountry { get; set; }
       
        /// <summary>
        /// 身分證號
        /// </summary>
        public string IDNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birth { get; set; }
        /// <summary>
        /// 性別
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 教育
        /// </summary>
        public int? Education { get; set; }
        /// <summary>
        /// 聯絡人電話
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 聯絡人關係
        /// </summary>
        public string ContactRelation { get; set; }
        /// <summary>
        /// 現居地區
        /// </summary>
        public string AddrCode { get; set; }
        /// <summary>
        /// 現居地址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 戶籍地區
        /// </summary>
        public string Domicile { get; set; }
        /// <summary>
        /// 新增日期
        /// </summary>
        public DateTime FillingDate { get; set; }
        /// <summary>
        /// 表單狀態
        /// </summary>
        public int Status { get; set; } 
        /// <summary>
        /// 問題ID
        /// </summary>
        public int QuestID { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        public string SequenceNum { get; set; }
        /// <summary>
        /// 問題答案
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 問題
        /// </summary>
        public X1_Report_Question Question { get; set; }
        /// <summary>
        /// Answer Main
        /// </summary>
        public X1_Report_Answer_Main AnswerM { get; set; }
    }
}
