
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
    
public partial class X1_Report_Answer_Main
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public X1_Report_Answer_Main()
    {

        this.X1_Report_Answer_Detail = new HashSet<X1_Report_Answer_Detail>();

        this.X1_Report_Answer_Extra = new HashSet<X1_Report_Answer_Extra>();

        this.X1_Report_Answer_File = new HashSet<X1_Report_Answer_File>();

        this.X1_Report_Export_File = new HashSet<X1_Report_Export_File>();

    }


    public int ID { get; set; }

    public Nullable<int> MainID { get; set; }

    public int ReportID { get; set; }

    public int PID { get; set; }

    public Nullable<int> OID { get; set; }

    public System.DateTime FillingDate { get; set; }

    public System.DateTime CreateDate { get; set; }

    public string CreateMan { get; set; }

    public System.DateTime ModifyDate { get; set; }

    public string ModifyMan { get; set; }

    public Nullable<System.DateTime> DeleteDate { get; set; }

    public string DeleteMan { get; set; }

    public bool IsDelete { get; set; }

    public bool Lock { get; set; }

    public int Status { get; set; }



    public virtual X1_PatientInfo X1_PatientInfo { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Answer_Detail> X1_Report_Answer_Detail { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Answer_Extra> X1_Report_Answer_Extra { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Answer_File> X1_Report_Answer_File { get; set; }

    public virtual X1_Report_Main X1_Report_Main { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Export_File> X1_Report_Export_File { get; set; }

}

}