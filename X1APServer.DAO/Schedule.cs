
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
    
public partial class Schedule
{

    public int ID { get; set; }

    public int PatientID { get; set; }

    public string ContentText { get; set; }

    public System.DateTime ReturnDate { get; set; }

    public System.DateTime CreateDate { get; set; }

    public string CreateMan { get; set; }

    public System.DateTime ModifyDate { get; set; }

    public string ModifyMan { get; set; }

    public Nullable<System.DateTime> DeleteDate { get; set; }

    public string DeleteMan { get; set; }

    public bool IsDelete { get; set; }



    public virtual X1_PatientInfo X1_PatientInfo { get; set; }

}

}
