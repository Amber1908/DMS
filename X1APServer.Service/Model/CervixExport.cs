using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Utils;

namespace X1APServer.Service.Model
{
    /// <summary>
    /// 子宮頸匯出資料格式
    /// </summary>
    public class CervixExport
    {
        /// <summary>
        /// 個案姓名 C(10)
        /// </summary>
        public string PTNAME { get; set; }
        /// <summary>
        /// 出生日期 C(08)
        /// </summary>
        public string PTBIRTH { get; set; }
        /// <summary>
        /// 身分證字號 C(10)
        /// </summary>
        public string PTID { get; set; }
        /// <summary>
        /// 教育程度 N(01)
        /// </summary>
        public int PTEDUCAT { get; set; }
        /// <summary>
        /// 現住址地區代碼 C(04)
        /// </summary>
        public string ADDCODEA { get; set; }
        /// <summary>
        /// 現住址戶籍代碼 C(04)
        /// </summary>
        public string ADDCODEB { get; set; }
        /// <summary>
        /// 支付方式 N(01)
        /// </summary>
        public int PTSUPPER { get; set; }
        /// <summary>
        /// 上次抹片檢查日期 N(01)
        /// </summary>
        public int LASTCHKD { get; set; }
        /// <summary>
        /// 病歷號 C(10)
        /// </summary>
        public string CHARTNO { get; set; }
        /// <summary>
        /// 抹片取樣日期 C(08)
        /// </summary>
        public string PASDATE { get; set; }
        /// <summary>
        /// 抹片採檢機構代碼 C(10)
        /// </summary>
        public string PASCODE { get; set; }
        /// <summary>
        /// 抹片判讀機構代碼 C(10)
        /// </summary>
        public string CHKCODE { get; set; }
        /// <summary>
        /// 抹片收到日期 C(08)
        /// </summary>
        public string CHKREC { get; set; }
        /// <summary>
        /// 抹片細胞病理編號 C(10)
        /// </summary>
        public string CHKNO { get; set; }
        /// <summary>
        /// 抹片品質 N(01)
        /// </summary>
        public int CHKQUL { get; set; }
        /// <summary>
        /// 抹片尚可或難以判讀原因1 C(01)
        /// </summary>
        public string CHKDIF { get; set; }
        /// <summary>
        /// 抹片尚可或難以判讀原因2 C(01)
        /// </summary>
        public string CHKDIF2 { get; set; }
        /// <summary>
        /// 可能的感染 C(06)
        /// </summary>
        public string CHKINF { get; set; }
        /// <summary>
        /// 細胞病理診斷結果 C(02)
        /// </summary>
        public string CHKDATA { get; set; }
        /// <summary>
        /// 資淺細胞醫檢師 C(02)
        /// </summary>
        public string JPATH { get; set; }
        /// <summary>
        /// 資深細胞醫檢師 C(02)
        /// </summary>
        public string SPATH { get; set; }
        /// <summary>
        /// 病理醫師 C(02)
        /// </summary>
        public string PATH { get; set; }
        /// <summary>
        /// 抹片確診日期 C(08)
        /// </summary>
        public string CHKSURED { get; set; }
        /// <summary>
        /// 採檢片數 N(01)
        /// </summary>
        public int CHKQTY { get; set; }
        /// <summary>
        /// 功能別 C(01)
        /// </summary>
        public string FUN_TYPE { get; set; }
        /// <summary>
        /// 健保申報註記 C(01)
        /// </summary>
        public string HFLAG { get; set; }
        /// <summary>
        /// 電話 C(10)
        /// </summary>
        public string PTTEL { get; set; }
        /// <summary>
        /// 地址 C(60)
        /// </summary>
        public string ADDR { get; set; }
        /// <summary>
        /// 國際疾病分類號(一) C(05)
        /// </summary>
        public string ICD9_1_OLD { get; set; }
        /// <summary>
        /// 國際疾病分類號(二) C(05)
        /// </summary>
        public string ICD9_2_OLD { get; set; }
        /// <summary>
        /// 國際疾病分類號(三) C(05)
        /// </summary>
        public string ICD9_3_OLD { get; set; }
        /// <summary>
        /// 健保卡就醫序號 C(02)
        /// </summary>
        public string CARDNO_2 { get; set; }
        /// <summary>
        /// 子宮是否已切除 C(01)
        /// </summary>
        public string ULTOMY { get; set; }
        /// <summary>
        /// 子宮是否接受過放射線治療 C(01)
        /// </summary>
        public string X_RAY { get; set; }
        /// <summary>
        /// 做抹片目的 C(01)
        /// </summary>
        public string PAS_PUR { get; set; }
        /// <summary>
        /// 檢體種類 C(01)
        /// </summary>
        public string SPL_TYPE { get; set; }
        /// <summary>
        /// 閱片方式 C(01)
        /// </summary>
        public string CHK_WAY { get; set; }
        /// <summary>
        /// 國籍別 C(01)
        /// </summary>
        public string NATIONALIT { get; set; }
        /// <summary>
        /// 健保IC卡就醫序號 C(04)
        /// </summary>
        public string CARDNO { get; set; }
        /// <summary>
        /// 現住址地區代碼次碼 C(04)
        /// </summary>
        public string ADDCODEC { get; set; }
        /// <summary>
        /// 抹片車或設站篩檢 C(01)
        /// </summary>
        public string CAR_STA { get; set; }
        /// <summary>
        /// 抹片檢體取樣人員 C(01)
        /// </summary>
        public string PRSN_TYPE { get; set; }
        /// <summary>
        /// 最近一次抹片檢查時間 C(01)
        /// </summary>
        public string LASTTIME { get; set; }
        /// <summary>
        /// 是否曾接種子宮頸癌疫苗人類乳突病毒(HPV)疫苗(95年起有HPV疫苗) C(01)
        /// </summary>
        public string VACCINE { get; set; }
        /// <summary>
        /// 接種疫苗年度 N(03)
        /// </summary>
        public string VACCINE_YY { get; set; }
        /// <summary>
        /// 醫檢師診斷結果 C(02)
        /// </summary>
        public string PRECHKDATA { get; set; }
        /// <summary>
        /// 衛生所醫療機構代碼 C(10)
        /// </summary>
        public string MEDIORG { get; set; }
        /// <summary>
        /// 做抹片目的 C(01)
        /// </summary>
        public string PURPOSE2 { get; set; }
        /// <summary>
        /// 有無自覺症狀如非月經期間不正常陰道出血及異常分泌物 C(01)
        /// </summary>
        public string SYMPTOM { get; set; }
        /// <summary>
        /// 國際疾病分類號(一) C(07)
        /// </summary>
        public string ICD9_1 { get; set; }
        /// <summary>
        /// 國際疾病分類號(二) C(07)
        /// </summary>
        public string ICD9_2 { get; set; }
        /// <summary>
        /// 國際疾病分類號(三) C(07)
        /// </summary>
        public string ICD9_3 { get; set; }
        /// <summary>
        /// 是否做過人類乳突病毒(HPV)檢測 C(01)
        /// </summary>
        public string HPV_TEST { get; set; }
        /// <summary>
        /// 複檢診斷結果 C(02)
        /// </summary>
        ///public string RECHKDATA { get; set; }

        /// <summary>
        /// 匯出國建署格式字串
        /// </summary>
        /// <returns></returns>
        public string ExportToString()
        {
            string ret = $"{PTNAME,-10}{PTBIRTH,-08}{PTID,-10}{PTEDUCAT,-01}{ADDCODEA,-04}{ADDCODEB,-04}{PTSUPPER,-01}{LASTCHKD,-01}{CHARTNO,-10}{PASDATE,-08}{PASCODE,-10}{CHKCODE,-10}{CHKREC,-08}{CHKNO,-10}{CHKQUL,-01}{CHKDIF,-01}{CHKDIF2,-01}{CHKINF,-06}{CHKDATA,-02}{JPATH,-02}{SPATH,-02}{PATH,-02}{CHKSURED,-08}{CHKQTY,-01}{FUN_TYPE,-01}{HFLAG,-01}{PTTEL,-10}{ADDR,-60}{ICD9_1_OLD,-05}{ICD9_2_OLD,-05}{ICD9_3_OLD,-05}{CARDNO_2,-02}{ULTOMY,-01}{X_RAY,-01}{PAS_PUR,-01}{SPL_TYPE,-01}{CHK_WAY,-01}{NATIONALIT,-01}{CARDNO,-04}{ADDCODEC,-04}{CAR_STA,-01}{PRSN_TYPE,-01}{LASTTIME,-01}{VACCINE,-01}{VACCINE_YY,-03}{PRECHKDATA,-02}{MEDIORG,-10}{PURPOSE2,-01}{SYMPTOM,-01}{ICD9_1,-07}{ICD9_2,-07}{ICD9_3,-07}{HPV_TEST,-01}";
            return ret;
        }

        /// <summary>
        /// 匯出國建署格式 big5 位元組
        /// </summary>
        /// <returns></returns>
        public byte[] ExportToBytes()
        {
            int index = 0;
            byte[] retByte = Enumerable.Repeat((byte)0x20, 267).ToArray();

            ROC.InsertBytes($"{PTNAME}", ref retByte, index, 10); index += 10;
            ROC.InsertBytes($"{PTBIRTH}", ref retByte, index, 08); index += 08;
            ROC.InsertBytes($"{PTID}", ref retByte, index, 10); index += 10;
            ROC.InsertBytes($"{PTEDUCAT}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{ADDCODEA}", ref retByte, index, 04); index += 04;
            ROC.InsertBytes($"{ADDCODEB}", ref retByte, index, 04); index += 04;
            ROC.InsertBytes($"{PTSUPPER}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{LASTCHKD}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{CHARTNO}", ref retByte, index, 10); index += 10;
            ROC.InsertBytes($"{PASDATE}", ref retByte, index, 08); index += 08;
            ROC.InsertBytes($"{PASCODE}", ref retByte, index, 10); index += 10;
            ROC.InsertBytes($"{CHKCODE}", ref retByte, index, 10); index += 10;
            ROC.InsertBytes($"{CHKREC}", ref retByte, index, 08); index += 08;
            ROC.InsertBytes($"{CHKNO}", ref retByte, index, 10); index += 10;
            ROC.InsertBytes($"{CHKQUL}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{CHKDIF}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{CHKDIF2}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{CHKINF}", ref retByte, index, 06); index += 06;
            ROC.InsertBytes($"{CHKDATA}", ref retByte, index, 02); index += 02;
            ROC.InsertBytes($"{JPATH}", ref retByte, index, 02); index += 02;
            ROC.InsertBytes($"{SPATH}", ref retByte, index, 02); index += 02;
            ROC.InsertBytes($"{PATH}", ref retByte, index, 02); index += 02;
            ROC.InsertBytes($"{CHKSURED}", ref retByte, index, 08); index += 08;
            ROC.InsertBytes($"{CHKQTY}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{FUN_TYPE}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{HFLAG}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{PTTEL}", ref retByte, index, 10); index += 10;
            ROC.InsertBytes($"{ADDR}", ref retByte, index, 60); index += 60;
            ROC.InsertBytes($"{ICD9_1_OLD}", ref retByte, index, 05); index += 05;
            ROC.InsertBytes($"{ICD9_2_OLD}", ref retByte, index, 05); index += 05;
            ROC.InsertBytes($"{ICD9_3_OLD}", ref retByte, index, 05); index += 05;
            ROC.InsertBytes($"{CARDNO_2}", ref retByte, index, 02); index += 02;
            ROC.InsertBytes($"{ULTOMY}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{X_RAY}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{PAS_PUR}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{SPL_TYPE}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{CHK_WAY}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{NATIONALIT}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{CARDNO}", ref retByte, index, 04); index += 04;
            ROC.InsertBytes($"{ADDCODEC}", ref retByte, index, 04); index += 04;
            ROC.InsertBytes($"{CAR_STA}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{PRSN_TYPE}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{LASTTIME}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{VACCINE}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{VACCINE_YY}", ref retByte, index, 03); index += 03;
            ROC.InsertBytes($"{PRECHKDATA}", ref retByte, index, 02); index += 02;
            ROC.InsertBytes($"{MEDIORG}", ref retByte, index, 10); index += 10;
            ROC.InsertBytes($"{PURPOSE2}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{SYMPTOM}", ref retByte, index, 01); index += 01;
            ROC.InsertBytes($"{ICD9_1}", ref retByte, index, 07); index += 07;
            ROC.InsertBytes($"{ICD9_2}", ref retByte, index, 07); index += 07;
            ROC.InsertBytes($"{ICD9_3}", ref retByte, index, 07); index += 07;
            ROC.InsertBytes($"{HPV_TEST}", ref retByte, index, 01); index += 01;

            return retByte;
        }
    }
}
