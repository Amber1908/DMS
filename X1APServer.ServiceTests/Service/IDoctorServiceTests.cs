using Microsoft.VisualStudio.TestTools.UnitTesting;
using X1APServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace X1APServer.Service.Tests
{
    [TestClass()]
    public class IDoctorServiceTests
    {
        [TestMethod()]
        public void GetUserTest()
        {
            string email = "williamchen@biomdcare.com";
            string exceptResult = "{\"email\":\"williamchen@biomdcare.com\",\"emailcheck\":1,\"pswd\":\"jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=\",\"displayname\":\"William\",\"firstname\":\"William\",\"lastname\":\"Chen\",\"country_code\":\"TW\",\"sex\":\"male\",\"birthday\":\"0001-01-01T00:00:00+08:00\",\"phone\":\"0000000000\",\"addr\":\"高雄市\",\"changepwsdtime\":\"2020-11-11T10:33:40.293+08:00\",\"status\":1,\"memo\":\"\",\"createtime\":\"2020-11-11T10:33:40.293+08:00\"}";
            string rspStr;
            var iDoctorSvc = new IDoctorService();

            var rsp = iDoctorSvc.GetUser(email).Result;
            rspStr = JsonConvert.SerializeObject(rsp);

            Assert.AreEqual(exceptResult, rspStr);
        }

        [TestMethod()]
        public void GetHealthWebTest()
        {
            int web_sn = 2;
            string exceptResult = "{\"web_sn\":2,\"web_name\":\"骨鬆管理站台\",\"web_db\":\"UBONE\",\"web_owner\":\"joetang@biomdcare.com\",\"web_count\":25,\"web_expire\":\"2020-08-28T10:46:28.257+08:00\",\"status\":1,\"logo\":0,\"memo\":\"123\",\"createtime\":\"2020-08-28T10:46:28.257+08:00\"}";
            string rspStr;
            var iDoctorSvc = new IDoctorService();

            var rsp = iDoctorSvc.GetHealthWeb(web_sn).Result;
            rspStr = JsonConvert.SerializeObject(rsp);

            Assert.AreEqual(exceptResult, rspStr);
        }

        [TestMethod()]
        public void GetUserByHealthWebTest()
        {
            int web_sn = 2;
            string exceptResult = "{\"web_sn\":2,\"web_name\":\"骨鬆管理站台\",\"web_db\":\"UBONE\",\"web_owner\":\"joetang@biomdcare.com\",\"web_count\":25,\"web_expire\":\"2020-08-28T10:46:28.257+08:00\",\"status\":1,\"logo\":0,\"memo\":\"123\",\"createtime\":\"2020-08-28T10:46:28.257+08:00\"}";
            string rspStr;
            var iDoctorSvc = new IDoctorService();

            var rsp = iDoctorSvc.GetUserByHealthWeb(web_sn).Result;
            rspStr = JsonConvert.SerializeObject(rsp);

            Assert.AreEqual(exceptResult, rspStr);
        }

        [TestMethod()]
        public void PopPusidTest()
        {
            Assert.Fail();
        }
    }
}