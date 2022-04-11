using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility;

namespace X1APServer.Repository
{
    public class X1_ReportETRepository : X1APBasicRepository<X1_Report_Export_Template>, IX1_ReportETRepository
    {
        public X1_ReportETRepository(X1APEntities dbContext) : base(dbContext)
        {
        }
    }
}
