using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    //X1_Morph_Main
    //public class X1_Morph_MainDAO : BasicRepository<X1_Morph_Main>, IX1_Morph_MainDAO
    //{
    //    public X1_Morph_MainDAO(X1APEntities dbContext) : base(dbContext)
    //    {
    //    }
    //}

    //X1_Morph_User
    //public class X1_Morph_UserDAO : BasicRepository<X1_Morph_User>, IX1_Morph_UserDAO
    //{
    //    public X1_Morph_UserDAO(X1APEntities dbContext) : base(dbContext)
    //    {
    //    }
    //}

    // X1_Config

    public class ReportExportFileRepository : X1APBasicRepository<X1_Report_Export_File>, IReportExportFileRepository
    {
        public ReportExportFileRepository(X1APEntities dbContext) : base(dbContext)
        {
        }
    }
}
