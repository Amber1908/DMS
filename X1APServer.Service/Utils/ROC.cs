using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Utils
{
    public static class ROC
    {
        public static bool TryParse(string dateString, out DateTime date)
        {
            char[] acceptSplitChar = new char[] { '-', '.', '/' };
            var dateAry = dateString.Split(acceptSplitChar);
            var rspDate = new DateTime();
            var rsp = false;
            int currentROCYear = DateTime.Now.Year - 1911;

            if (dateAry.Length == 3)
            {
                int year, month, tmpdate;

                if (int.TryParse(dateAry[0], out year) &&
                    int.TryParse(dateAry[1], out month) &&
                    int.TryParse(dateAry[2], out tmpdate) &&
                    year <= currentROCYear)
                {
                    try
                    {
                        year += 1911;
                        rspDate = new DateTime(year, month, tmpdate);
                        rsp = true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                    }
                }
            }

            date = rspDate;
            return rsp;
        }

        /// <summary>
        /// YYY-MM-DD > YYYYMMDD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CDate2WDate(string input)
        {
            string output = "";
            string outTemp;

            if(input != null && input.Length > 6)
            {
                outTemp = input.Replace("-", "").Replace("/", "");
                output += (int.Parse(outTemp.Substring(0, 3)) + 1911).ToString();
                output += outTemp.Substring(3);
            }

            return output;
        }

        #region 轉換BIG5
        /// <summary>
        /// 轉換BIG5
        /// </summary>
        /// <param name=”strUtf”>輸入UTF-8</param>
        /// <returns></returns>
        public static string ConvertBig5(string strUtf)
        {
            Encoding utf81 = Encoding.GetEncoding("utf-8");
            Encoding big51 = Encoding.GetEncoding("big5");
            byte[] strUtf81 = utf81.GetBytes(strUtf.Trim());
            byte[] strBig51 = Encoding.Convert(utf81, big51, strUtf81);

            char[] big5Chars1 = new char[big51.GetCharCount(strBig51, 0, strBig51.Length)];
            big51.GetChars(strBig51, 0, strBig51.Length, big5Chars1, 0);
            string tempString1 = new string(big5Chars1);
            return tempString1;
        }

        public static bool InsertBytes(string strUtf, ref byte[] des, int index, int length)
        {
            bool ret = false;
            byte[] tmp;

            try
            {
                if(strUtf != null && strUtf.Length > 0)
                {
                    tmp = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("big5"), Encoding.UTF8.GetBytes(strUtf));
                    Buffer.BlockCopy(tmp, 0, des, index, tmp.Length);
                }
                ret = true;
            }
            catch
            {
                ret = false;
            }

            return ret;
        }
        #endregion
    }
}
