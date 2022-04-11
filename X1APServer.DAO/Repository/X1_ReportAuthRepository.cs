using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportAuthRepository : X1APBasicRepository<X1_Report_Authorization>, IX1_ReportAuthRepository
    {
        public X1_ReportAuthRepository(X1APEntities dbContext) : base(dbContext)
        {

        }
        
        public IQueryable<X1_Report_Authorization> GetAllAuth(int? reportID)
        {
            var filterAuth = GetAll();

            if (reportID.HasValue)
            {
                filterAuth = filterAuth.Where(x => x.ReportID == reportID);
            }

            return filterAuth;
        }
    }
}
