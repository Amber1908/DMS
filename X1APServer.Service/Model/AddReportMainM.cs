using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddReportMainM
    {
        public class AddReportMainReq : REQBase
        {
            /// <summary>
            /// 問卷結構
            /// </summary>
            public QuestionnaireStructure Structure { get; set; }
            /// <summary>
            /// 是否為同個類別的新版本
            /// </summary>
            public bool IsNewVersion { get; set; } = false;

            private DateTime? reserveDate;
            /// <summary>
            /// 預定發佈時間
            /// </summary>
            public DateTime? ReserveDate { 
                get {
                    return reserveDate;
                }
                set
                {
                    if (!value.HasValue)
                        value = DateTime.Now;

                    reserveDate = value.Value;
                } 
            }
        }

        public class AddReportMainRsp : RSPBase
        {
            /// <summary>
            /// Report Main ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 報告類別
            /// </summary>
            public string Category { get; set; }
            /// <summary>
            /// 問卷結構
            /// </summary>
            public QuestionnaireStructure Structure { get; set; }
        }

        public class QuestionnaireStructure
        {
            /// <summary>
            /// Report Main ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 表單標題
            /// </summary>
            [Required]
            [MaxLength(1000)]
            public string Title { get; set; }
            /// <summary>
            /// 表單類別
            /// </summary>
            //[Required]
            public string Category { get; set; }
            /// <summary>
            /// 表單描述
            /// </summary>
            [MaxLength(3000)]
            public string Description { get; set; }
            /// <summary>
            /// 群組清單
            /// </summary>
            public List<Group> Children { get; set; }
            /// <summary>
            /// 排序編號
            /// </summary>
            public int IndexNum { get; set; }
        }

        public class Group
        {
            public int ID { get; set; }
            /// <summary>
            /// 類型(問題或群組)
            /// </summary>
            [Required]
            public string Type { get; set; }
            /// <summary>
            /// 群組標題
            /// </summary>
            [Required]
            [MaxLength(1000)]
            public string Title { get; set; }
            /// <summary>
            /// 群組描述
            /// </summary>
            [MaxLength(3000)]
            public string Description { get; set; }
            /// <summary>
            /// 問題清單
            /// </summary>
            public List<Item> Children { get; set; }
        }
        /// <summary>
        /// 問題
        /// </summary>
        public class Item
        {
            public int ID { get; set; }
            /// <summary>
            /// 類型(問題或群組)
            /// </summary>
            [Required]
            public string Type { get; set; }
            /// <summary>
            /// 問題類型
            /// </summary>
            [Required]
            public string QuestionType { get; set; }
            /// <summary>
            /// 問題文字
            /// </summary>
            [Required]
            [MaxLength(1000)]
            public string QuestionText { get; set; }
            /// <summary>
            /// 問題描述
            /// </summary>
            [MaxLength(3000)]
            public string Description { get; set; }
            /// <summary>
            /// 問題選項
            /// </summary>
            public List<Answeroption> AnswerOption { get; set; }
            [Required]
            public bool Required { get; set; }
            /// <summary>
            /// 是否擁有其他選項
            /// </summary>
            public bool OtherAns { get; set; }
            /// <summary>
            /// 其他選項問題ID
            /// </summary>
            public int? OtherAnsID { get; set; }
            public string Image { get; set; }
            /// <summary>
            /// 排序編號
            /// </summary>
            [Required]
            public int CodingBookIndex { get; set; }
            public string CodingBookTitle { get; set; }
            /// <summary>
            /// 問題編號
            /// </summary>
            [MaxLength(50)]
            public string QuestionNo { get; set; }
            /// <summary>
            /// 是否關注
            /// </summary>
            public bool Pin { get; set; }
            /// <summary>
            /// 關注ID
            /// </summary>
            public int? PinnedID { get; set; }
            /// <summary>
            /// 關注名稱
            /// </summary>
            public string PinnedName { get; set; }
            /// <summary>
            /// 區段條件清單
            /// </summary>
            public List<ValidationGroup> ValidationGroupList { get; set; }
        }

        /// <summary>
        /// 選項
        /// </summary>
        public class Answeroption
        {
            public int ID { get; set; }
            /// <summary>
            /// 選項文字
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string OptionText { get; set; }
            /// <summary>
            /// 選項值
            /// </summary>
            [Required]
            [MaxLength(100)]
            public string Value { get; set; }
            /// <summary>
            /// 排序編號
            /// </summary>
            public int CodingBookIndex { get; set; }
            public string CodingBookTitle { get; set; }
            /// <summary>
            /// 從表單編輯器隱藏
            /// </summary>
            public bool HiddenFromBackend { get; set; } = false;
            /// <summary>
            /// 是否為異常值
            /// </summary>
            public string AbnormalValue { get; set; }
            /// <summary>
            /// 異常顏色
            /// </summary>
            [MaxLength(50)]
            public string AbnormalColor { get; set; }
        }

        /// <summary>
        /// 標準值群組
        /// </summary>
        public class ValidationGroup
        {
            /// <summary>
            /// 群組編號
            /// </summary>
            public int? GroupNum { get; set; }
            /// <summary>
            /// 比較屬性名稱，ex. 性別、年齡
            /// </summary>
            //[Required]
            [MaxLength(50)]
            public string AttributeName { get; set; }
            /// <summary>
            /// 運算子1
            /// </summary>
            //[Required]
            [MaxLength(50)]
            public string Operator1 { get; set; }
            /// <summary>
            /// 運算元1
            /// </summary>
            //[Required]
            [MaxLength(50)]
            public string Value1 { get; set; }
            /// <summary>
            /// 運算子2
            /// </summary>
            [MaxLength(50)]
            public string Operator2 { get; set; }
            /// <summary>
            /// 運算元2
            /// </summary>
            [MaxLength(50)]
            public string Value2 { get; set; }
            /// <summary>
            /// 區段條件清單
            /// </summary>
            public List<Validation> ValidationList { get; set; } = new List<Validation>();
        }

        /// <summary>
        /// 區段條件
        /// </summary>
        public class Validation
        {
            /// <summary>
            /// 運算子
            /// </summary>
            [Required]
            [MaxLength(50)]
            public string Operator { get; set; }
            /// <summary>
            /// 運算元
            /// </summary>
            [Required]
            [MaxLength(50)]
            public string Value { get; set; }
            /// <summary>
            /// 符合時顯示顏色
            /// </summary>
            [MaxLength(50)]
            public string Color { get; set; }
            /// <summary>
            /// 是否為正常值
            /// </summary>
            public bool Normal { get; set; } = false;
            /// <summary>
            /// 群組編號
            /// </summary>
            public int? GroupNum { get; set; }
        }
    }
}
