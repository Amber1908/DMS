
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
    
public partial class ExportTemplateExtraQuest
{

    public int ID { get; set; }

    public int ExportTemplateID { get; set; }

    public string QuestName { get; set; }

    public string QuestText { get; set; }

    public string QuestType { get; set; }

    public bool Required { get; set; }



    public virtual X1_Report_Export_Template X1_Report_Export_Template { get; set; }

}

}
