using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHRI.DataRepository.Utility.Formatter
{
    public class OneLineFormatter : DatabaseLogFormatter
    {
        public OneLineFormatter(DbContext context, Action<string> writeAction)
            : base(context, writeAction)
        {
        }

        public override void LogResult<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
        }
    }
}
