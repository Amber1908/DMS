using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class CervixTable
    {
        /// <summary>
        /// 檢驗單 ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 檢驗單模組 ID
        /// </summary>
        public int ReportID { get; set; }
        /// <summary>
        /// 採檢日
        /// </summary>
        public DateTime FillingDate { get; set; }
        /// <summary>
        /// 開單日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 異動日期
        /// </summary>
        public DateTime ModifyDate { get; set; }
        /// <summary>
        /// 表單狀態 1待檢驗、2檢驗中、3待覆核、4覆核中、5已覆核、6已結案、7已匯出
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 個案資料
        /// </summary>
        public CervixCase cervixCase { get; set; }
        /// <summary>
        /// 問券內容
        /// </summary>
        public List<CervixQuestion> cervixQuestions { get; set; }
    }

    public class CervixCase
    {
        /// <summary>
        /// 個案 ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 個案國籍 1:本國 2:外籍人士
        /// </summary>
        public int PUCountry { get; set; }
        /// <summary>
        /// 個案姓名
        /// </summary>
        public string PUName { get; set; }
        /// <summary>
        /// 個案生日
        /// </summary>
        public DateTime? PUDOB { get; set; }
        /// <summary>
        /// 身分證號
        /// </summary>
        public string IDNo { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// 教育：1 無 2 小學 3 國（初）中 4 高中、高職 5 專科、大學 6 研究所以上 7 拒答
        /// </summary>
        public int? Education { get; set; }
        /// <summary>
        /// 現住址地區代碼
        /// </summary>
        public string AddrCode { get; set; }
        /// <summary>
        /// 所屬衛生所醫療機構代碼
        /// </summary>
        public string HCCode { get; set; }
        /// <summary>
        /// 現住址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 現住址戶籍代碼
        /// </summary>
        public string Domicile { get; set; }
    }

    public class CervixQuestion
    {
        /// <summary>
        /// 問題 ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 問題編碼
        /// </summary>
        public string QuestionNo { get; set; }
        /// <summary>
        /// 問題類型
        /// </summary>
        public int QuestionType { get; set; }
        /// <summary>
        /// 問題題目
        /// </summary>
        public string QuestionText { get; set; }
        /// <summary>
        /// 附註說明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 答案選項  ( Json )
        /// </summary>
        public string AnswerOption { get; set; }
        /// <summary>
        /// 答案 ID
        /// </summary>
        public int AID { get; set; }
        /// <summary>
        /// 回答內容
        /// </summary>
        public string Value { get; set; }
    }
}
