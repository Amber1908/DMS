
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
    
public partial class X1_Report_Main
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public X1_Report_Main()
    {

        this.X1_Report_Answer_Main = new HashSet<X1_Report_Answer_Main>();

        this.X1_Report_Authorization = new HashSet<X1_Report_Authorization>();

        this.X1_Report_Question_File = new HashSet<X1_Report_Question_File>();

        this.X1_Report_Question_Group = new HashSet<X1_Report_Question_Group>();

        this.X1_Report_Question = new HashSet<X1_Report_Question>();

    }


    public int ID { get; set; }

    public int IndexNum { get; set; }

    public string Category { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string OutputJson { get; set; }

    public System.DateTime CreateDate { get; set; }

    public string CreateMan { get; set; }

    public System.DateTime ModifyDate { get; set; }

    public string ModifyMan { get; set; }

    public Nullable<System.DateTime> DeleteDate { get; set; }

    public string DeleteMan { get; set; }

    public bool IsDelete { get; set; }

    public Nullable<System.DateTime> PublishDate { get; set; }

    public bool IsPublish { get; set; }

    public System.DateTime ReserveDate { get; set; }

    public string FuncCode { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Answer_Main> X1_Report_Answer_Main { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Authorization> X1_Report_Authorization { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Question_File> X1_Report_Question_File { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Question_Group> X1_Report_Question_Group { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<X1_Report_Question> X1_Report_Question { get; set; }

}

}
