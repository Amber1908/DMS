using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Repository
{
    public partial class X1APEntities : DbContext
    {
        public X1APEntities(string connectionString) : base(connectionString)
        {

        }
    }
}
