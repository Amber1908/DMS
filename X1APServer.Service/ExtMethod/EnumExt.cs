using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.ExtMethod
{
    public enum PatientInfoKey
    {
        [Description("國籍")]
        PUCountry,
        [Description("身分證字號,量測人ID")]
        IdNo,
        [Description("姓名")]
        Name,
        [Description("性別")]
        Gender,
        [Description("生日")]
        Birth,
        [Description("電話")]
        Cellphone,
        [Description("緊急聯絡人電話")]
        ContactPhone,
        [Description("緊急聯絡人關係")]
        ContactRelation,
        [Description("現居地區")]
        AddrCode,
        [Description("所屬衛生所醫療機構")]
        HCCode,
        [Description("戶籍地區")]
        Domicile,
        [Description("現居完整地址")]
        Addr,
        [Description("教育")]
        Education,
        [Description("表單狀態")]
        Status,
        [Description("填寫日期,測試時間")]
        FillingDate,
       
    }

    public static class EnumExt
    {
        public static string GetDescriptionText(this PatientInfoKey source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
             typeof(DescriptionAttribute), false);
            if (attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
