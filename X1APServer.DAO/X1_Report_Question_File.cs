
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
    
public partial class X1_Report_Question_File
{

    public int ID { get; set; }

    public int RMID { get; set; }

    public int RQID { get; set; }

    public byte[] FileData { get; set; }

    public string FileName { get; set; }

    public string MimeType { get; set; }



    public virtual X1_Report_Main X1_Report_Main { get; set; }

    public virtual X1_Report_Question X1_Report_Question { get; set; }

}

}
