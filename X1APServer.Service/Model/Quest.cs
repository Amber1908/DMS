using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;

namespace X1APServer.Service.Model
{
    public class Quest
    {
        public int ReportID { get; set; }
        public int ID { get; set; }
        public string CodingBookTitle { get; set; }
        public int CodingBookIndex { get; set; }
        public X1_Report_Question Question { get; set; }
    }
}
