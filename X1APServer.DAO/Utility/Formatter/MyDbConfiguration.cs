using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHRI.DataRepository.Utility.Formatter
{
    public class MyDbConfiguration : DbConfiguration
    {
        public MyDbConfiguration()
        {
            SetDatabaseLogFormatter(
                (context, writeAction) => new OneLineFormatter(context, writeAction));
        }
    }
}
