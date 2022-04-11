using iDoctorTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Model
{
    public class AddWebSettingM
    {
        public class Response : RSPBase
        {
            public PUSID Data { get; set; }
        }
    }
}
