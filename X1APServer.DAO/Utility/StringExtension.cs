using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace X1APServer.Repository.Utility
{
    public static class StringExtension
    {
        /// <summary>
        /// 加強Substring功能，長度輸入錯誤時，可容錯，並回傳範圍內的有效值
        /// </summary>
        /// <param name="stringObject"></param>
        /// <param name="startIndex">起始index</param>
        /// <param name="length">長度</param>
        /// <returns></returns>
        public static string SubstringEx(this string stringObject, int startIndex, int length)
        {
            int intLen = stringObject.Length;
            int intSubLen = intLen - startIndex;
            string strReturn;
            if (length == 0)
                strReturn = "";
            else
            {
                if (intLen <= startIndex)
                    strReturn = "";
                else
                {
                    if (length > intSubLen)
                        length = intSubLen;

                    strReturn = stringObject.Substring(startIndex, length);
                }
            }
            return strReturn;
        }

        /// <summary>
        /// 加強Substring功能，可處理中文字，每個中文字視為 2 Bytes
        /// 修正標準的Substring把中文字視為 1 Bytes
        /// </summary>
        /// <param name="a_SrcStr"></param>
        /// <param name="startIndex">起始index</param>
        /// <param name="length">長度</param>
        /// <returns></returns>
        public static string SubstrBig5(this string a_SrcStr, int startIndex, int length)
        {
            Encoding l_Encoding = Encoding.GetEncoding("big5", new EncoderExceptionFallback(), new DecoderReplacementFallback(""));
            byte[] l_byte = l_Encoding.GetBytes(a_SrcStr);
            if (length <= 0)
                return "";
            //例若長度10 
            //若a_StartIndex傳入9 -> ok, 10 ->不行 
            if (startIndex + 1 > l_byte.Length)
                return "";
            else
            {
                //若a_StartIndex傳入9 , a_Cnt 傳入2 -> 不行 -> 改成 9,1 
                if (startIndex + length > l_byte.Length)
                    length = l_byte.Length - startIndex;
            }
            return l_Encoding.GetString(l_byte, startIndex, length);
        }

        public static DateTime ToDateTime(this string s, string formatString)
        {
            try
            {
                if (s.IsNullOrEmpty())
                {
                    return DateTime.UtcNow;
                }
                else
                {
                    return DateTime.ParseExact(s, formatString, null);
                }
            }
            catch (Exception ex)
            {
                return DateTime.UtcNow;
            }
        }

        public static bool IsValidateIPv4(this string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        /// <summary>
        /// 檢驗是否為有效Email
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(this string s)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(s);
        }

        /// <summary>
        /// 檢驗是否為有效電話
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsValidPhone(this string p)
        {
            return new Regex("^09[0-9]{8}$").IsMatch(p);
        }

        /// <summary>
        /// 檢驗是否為有效大陸電話
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static bool IsChinaPhone(this string cp)
        {
            return new Regex("^1[3|4|5|7|8][0-9]{9}$").IsMatch(cp);
        }
        /// <summary>
        /// 檢驗是否為有效Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string url)
        {
            string strRegex = "^(https?://)"
        + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
        + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
        + "|" // allows either IP or domain
        + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
        + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
        + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
        + "(:[0-9]{1,5})?" // port number- :80
        + "((/?)|" // a slash isn't required if there is no file name
        + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
            return new Regex(strRegex).IsMatch(url);
        }

        /// <summary>
        /// 檢驗提供的網址是否存在
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public static bool UrlAvailable(this string httpUrl)
        {
            if (!httpUrl.StartsWith("http://") && !httpUrl.StartsWith("https://"))
                httpUrl = "http://" + httpUrl;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(httpUrl);
                myRequest.Method = "GET";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse myHttpWebResponse =
                   (HttpWebResponse)myRequest.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 將字串反向排列
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Reverse(this string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }

        /// <summary>
        /// 用來提供縮寫，可用於預覽內容
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count">預覽長度</param>
        /// <param name="endings">串接縮寫的文字，例:"..."</param>
        /// <returns></returns>
        public static string Reduce(this string s, int count, string endings)
        {
            if (count < endings.Length)
                throw new Exception("Failed to reduce to less than endings length.");
            int sLength = s.Length;
            int len = sLength;
            if (endings != null)
                len += endings.Length;
            if (count > sLength)
                return s; //it's too short to reduce
            s = s.Substring(0, count);
            if (endings != null)
                s += endings;
            return s;
        }

        /// <summary>
        /// 檢查是否為數字
        /// </summary>
        /// <param name="s"></param>
        /// <param name="floatpoint">是否含浮點文字</param>
        /// <returns></returns>
        public static bool IsNumber(this string s, bool floatpoint)
        {
            int i;
            double d;
            string withoutWhiteSpace = s.RemoveSpaces();
            if (floatpoint)
                return double.TryParse(withoutWhiteSpace, NumberStyles.Any,
                    Thread.CurrentThread.CurrentUICulture, out d);
            else
                return int.TryParse(withoutWhiteSpace, out i);
        }

        /// <summary>
        /// 移除字串內含的空白
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveSpaces(this string s)
        {
            return s.Replace(" ", "");
        }

        /// <summary>
        /// 即string.IsNullOrEmpty(str)，可以直接套用在字串
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string theString)
        {
            return string.IsNullOrEmpty(theString);
        }

        public static bool ToBool(this string s, bool defaultValue = false)
        {
            if (s.IsNullOrEmpty())
            {
                return defaultValue;
            }
            else
            {
                bool boolConverted = defaultValue;
                bool.TryParse(s, out boolConverted);
                return boolConverted;
            }
        }

        public static int ToInt(this string s, int defaultValue = 0)
        {
            if (s.IsNullOrEmpty())
            {
                return defaultValue;
            }
            else
            {
                s = s.Replace(".00", "");
                s = s.Replace(".0", "");
            }
            int intConverted = 0;
            int.TryParse(s, out intConverted);
            if (intConverted == 0)
            {
                intConverted = defaultValue;
            }
            return intConverted;
        }

        public static Int64 ToInt64(this string s, Int64 defaultValue = 0)
        {
            if (s.IsNullOrEmpty())
            {
                return defaultValue;
            }
            else
            {
                s = s.Replace(".00", "");
            }
            Int64 intConverted = 0;
            Int64.TryParse(s, out intConverted);
            if (intConverted == 0)
            {
                intConverted = defaultValue;
            }
            return intConverted;
        }

        public static double ToDouble(this string s)
        {
            double dConverted = 0;
            double.TryParse(s, out dConverted);
            return dConverted;
        }

        public static decimal ToDecimal(this string s, decimal defaultValue = 0)
        {
            decimal dConverted = 0;
            if (decimal.TryParse(s, out dConverted))
            {
                return dConverted;
            }
            return defaultValue;
        }

        public static long ToLong(this string s)
        {
            long longConverted = 0;
            long.TryParse(s, out longConverted);
            return longConverted;
        }

        public static string ToString(this object s, string defaultString)
        {
            if (s == null)
            {
                return defaultString;
            }
            else
            {
                return s.ToString();
            }
        }

        public static bool IsValidJson<T>(this string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<T>(strInput);
                    return true;
                }
                catch (Exception ex) // not valid
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
