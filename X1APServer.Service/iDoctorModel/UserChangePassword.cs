using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.iDoctorModel
{
    public class UserChangePassword
    {
        /// <summary>
        /// 使用者E-mail
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 使用者舊密碼
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 使用者新密碼
        /// </summary>
        public string newpassword { get; set; }
    }
}
