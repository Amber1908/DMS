
//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------


namespace X1APServer.Repository
{

using System;
    using System.Collections.Generic;
    
public partial class ValidationCondition
{

    public int ID { get; set; }

    public int QuestionID { get; set; }

    public string AttributeName { get; set; }

    public string Value1 { get; set; }

    public string Operator1 { get; set; }

    public string Value2 { get; set; }

    public string Operator2 { get; set; }

    public int GroupNum { get; set; }



    public virtual X1_Report_Question X1_Report_Question { get; set; }

}

}