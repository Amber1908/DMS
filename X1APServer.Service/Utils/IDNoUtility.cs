using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace X1APServer.Service.Utils
{
    public static class IDNoUtility
    {
        //public static bool CheckIDNo(string idNo)
        //{
        //    char[] charSeq = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'W', 'Z', 'I', 'O' };
        //    int[] charWeight = new int[] { 1, 9, 8, 7, 6, 5, 4, 3, 2, 1, 1 };
        //    int startNum = 10;

        //    Regex rx = new Regex(@"^[A-Z][0-9]{9}$", RegexOptions.Compiled);
        //    if (!rx.IsMatch(idNo)) return false;
                
        //    int charIndex = Array.IndexOf(charSeq, idNo[0]);
        //    int number = charIndex + startNum;
        //    string newStr = number + idNo.Substring(1);
        //    int sum = 0;

        //    for (int i = 0; i < newStr.Length; i++)
        //    {
        //        int charNum = int.Parse(newStr[i].ToString());
        //        sum += charNum * charWeight[i];
        //    }

        //    if (sum % 10 == 0) return true;
        //    else return false;
        //}

        /// <summary>
        /// 判斷身分證號及統一證號是否正確，並判斷性別及國籍
        ///  20210830修改 外籍人士身分證號比照國民身分證號 第二碼8為男 第二碼9為女
        /// 國籍
        /// 本署核發之外來人口統一證號編碼，共計10碼，前2碼使用英文字母，
        /// 第1碼為區域碼（同國民身分證註1）
        /// 第2碼為性別碼(註 2)、3至10碼為阿拉伯數字，其中第3至9碼為流水號、第10碼為檢查號碼。
        /// 註1：英文字母代表直轄市、縣、市別：
        /// 台北市 A、台中市 B、基隆市 C、台南市 D、高雄市 E
        /// 新北市 F、宜蘭縣 G、桃園縣 H、嘉義市 I、新竹縣 J
        /// 苗栗縣 K、原台中縣 L、南投縣 M、彰化縣 N、新竹市 O
        /// 雲林縣 P、嘉義縣 Q、原台南縣 R、原高雄縣 S、屏東縣 T
        /// 花蓮縣 U、台東縣 V、金門縣 W、澎湖縣 X、連江縣 Z
        /// 註2：
        /// 臺灣地區無戶籍國民、大陸地區人民、港澳居民：
        /// 男性使用A、女性使用B
        /// 外國人：
        /// 男性使用C、女性使用D
        /// 第二碼8為男 第二碼9為女
        /// </summary>
        /// <param name="str"></param>
        public static bool CheckIDNo(String str)
        {
            string sex = "";
            string nationality = "";
            if (str == null || string.IsNullOrWhiteSpace(str) || str.Length != 10)
            {
                return false;
            }
            char[] pidCharArray = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            str = str.ToUpper(); // 轉換大寫
            char[] strArr = str.ToCharArray(); // 字串轉成char陣列
            int verifyNum = 0;

            // string pat = @"[A-Z]{1}[1-2]{1}[0-9]{8}";
            string pat = @"[A-Z]{1}[1289]{1}[0-9]{8}";   // 20210830修改 外籍人士身分證號比照國民身分證號 第二碼8為男 第二碼9為女
                                                         // Instantiate the regular expression object.
            Regex rTaiwan = new Regex(pat, RegexOptions.IgnoreCase);
            // Match the regular expression pattern against a text string.
            Match mTaiwan = rTaiwan.Match(str);
            // 檢查身分證字號
            if (mTaiwan.Success)
            {
                // 原身分證英文字應轉換為10~33，這裡直接作個位數*9+10
                int[] pidIDInt = { 1, 10, 19, 28, 37, 46, 55, 64, 39, 73, 82, 2, 11, 20, 48, 29, 38, 47, 56, 65, 74, 83, 21, 3, 12, 30 };
                // 第一碼
                verifyNum = verifyNum + pidIDInt[Array.BinarySearch(pidCharArray, strArr[0])];
                // 第二~九碼
                for (int i = 1, j = 8; i < 9; i++, j--)
                {
                    verifyNum += Convert.ToInt32(strArr[i].ToString(), 10) * j;
                }
                // 檢查碼
                verifyNum = (10 - (verifyNum % 10)) % 10;
                bool ok = verifyNum == Convert.ToInt32(strArr[9].ToString(), 10);
                if (ok)
                {
                    // 判斷性別 & 國籍
                    sex = "男";
                    if (strArr[1] == '2' || strArr[1] == '9') sex = "女";  // 20210830 修改
                    nationality = "本國籍";

                    if (strArr[1] == '8' || strArr[1] == '9') nationality = "外籍人士";  // 20210830 修改
                }
                return ok;
            }

            // 檢查統一證號(居留證)
            verifyNum = 0;
            pat = @"[A-Z]{1}[A-D]{1}[0-9]{8}";
            // Instantiate the regular expression object.
            Regex rForeign = new Regex(pat, RegexOptions.IgnoreCase);
            // Match the regular expression pattern against a text string.
            Match mForeign = rForeign.Match(str);
            if (mForeign.Success)
            {
                // 原居留證第一碼英文字應轉換為10~33，十位數*1，個位數*9，這裡直接作[(十位數*1) mod 10] + [(個位數*9) mod 10]
                int[] pidResidentFirstInt = { 1, 10, 9, 8, 7, 6, 5, 4, 9, 3, 2, 2, 11, 10, 8, 9, 8, 7, 6, 5, 4, 3, 11, 3, 12, 10 };
                // 第一碼
                verifyNum += pidResidentFirstInt[Array.BinarySearch(pidCharArray, strArr[0])];
                // 原居留證第二碼英文字應轉換為10~33，並僅取個位數*6，這裡直接取[(個位數*6) mod 10]
                int[] pidResidentSecondInt = { 0, 8, 6, 4, 2, 0, 8, 6, 2, 4, 2, 0, 8, 6, 0, 4, 2, 0, 8, 6, 4, 2, 6, 0, 8, 4 };
                // 第二碼
                verifyNum += pidResidentSecondInt[Array.BinarySearch(pidCharArray, strArr[1])];
                // 第三~八碼
                for (int i = 2, j = 7; i < 9; i++, j--)
                {
                    verifyNum += Convert.ToInt32(strArr[i].ToString(), 10) * j;
                }
                // 檢查碼
                verifyNum = (10 - (verifyNum % 10)) % 10;
                bool ok = verifyNum == Convert.ToInt32(strArr[9].ToString(), 10);
                if (ok)
                {
                    // 判斷性別 & 國籍
                    sex = "男";
                    if (strArr[1] == 'B' || strArr[1] == 'D') sex = "女";
                    nationality = "外籍人士";
                    if (strArr[1] == 'A' || strArr[1] == 'B') nationality += "(臺灣地區無戶籍國民、大陸地區人民、港澳居民)";
                }
                return ok;
            }
            return false;
        }
    }
}
