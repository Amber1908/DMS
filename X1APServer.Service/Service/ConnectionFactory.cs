using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Connection
{
    public class ConnectionFactory
    {
        public IEnumerable CreateConnection(string qu)
        {
            var ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DMSShareEntities"].ConnectionString;

            using (var conn=new SqlConnection(ConnectionString))
            {
                conn.Open();
               var result= conn.Query<dynamic>(qu);
               return result;
            }
           
        }
    }
}