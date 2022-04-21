using iDoctorTools.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using X1APServer.Service.Misc;
using X1APServer.Service.Model;
using X1APServer.Service.Interface;
using X1APServer.Service.Utils;

namespace X1APServer.Service
{
    public class IDoctorService : IIDoctorService
    {
        private static readonly CustomHttpClient client = new CustomHttpClient();
        private static readonly string idoctorAPIUrl = ConfigurationManager.AppSettings["IDoctorAPIUrl"];

        public IDoctorService()
        {
            string authenticationValue = Base64Utility.ToBase64String("HealthWeb:42710833");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authenticationValue);
        }

        public async Task<bool> CheckSessionAsync(string sessionkey, int web_sn)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/CheckSession");
            builder.AddOrUpdateParam("sessionkey", sessionkey);
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = false;
            bool.TryParse(responseBody, out rspObj);
            return rspObj;
        }

        public async Task<SESSION> GenSession(string email, int web_sn)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/GenSession");
            builder.AddOrUpdateParam("email", email);
            builder.AddOrUpdateParam("web_sn", web_sn.ToString());
            builder.AddOrUpdateParam("x1code", "bio000d");
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<SESSION>(responseBody);
            return rspObj;
        }

        public async Task<HEALTHWEB> GetHealthWeb(int web_sn)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/GetHealthWeb");
            builder.AddOrUpdateParam("web_sn", web_sn.ToString());
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<HEALTHWEB>(responseBody);
           
            return rspObj;
        }

        public async Task<GetHealthWebByUserM.Response> GetHealthWebByUser(string email)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/GetHealthWebByUser");
            builder.AddOrUpdateParam("email", email);
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<List<HEALTHWEB>>(responseBody);
            return new GetHealthWebByUserM.Response()
            {
                Data = rspObj
            };
        }

        public async Task<USER> GetUser(string email)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/GetUser");
            builder.AddOrUpdateParam("email", email);
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<USER>(responseBody);
            return rspObj;
        }

        public async Task<List<USER>> GetUserByHealthWeb(int web_sn)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/GetUserByHealthWeb");
            builder.AddOrUpdateParam("web_sn", web_sn.ToString());
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<List<USER>>(responseBody);
            return rspObj;
        }

        public async Task<PUSID> PopPusid(string pusid)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/PopPusid");
            builder.AddOrUpdateParam("pusid", pusid);
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<PUSID>(responseBody);
            return rspObj;
        }

        public async Task<RSPBase> UserChangePassword(string email, string password, string newpassword)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/UserChangePassword");
            builder.AddOrUpdateParam("email", email);
            builder.AddOrUpdateParam("password", password);
            builder.AddOrUpdateParam("newpassword", newpassword);
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<ActionResultModel>(responseBody);
            return IDoctorResponseConverter.Convert(rspObj);
        }

        public async Task<ActionResultModel> UserLogin(string email, string password)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/UserLogin");
            builder.AddOrUpdateParam("email", email);
            builder.AddOrUpdateParam("password", password);
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<ActionResultModel>(responseBody);
            return rspObj;
        }

        public async Task<List<AREACODE>> GetAreaCode()
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/GetAreaCode");
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<List<AREACODE>>(responseBody);
            return rspObj;
        }

        public async Task<List<HOSPITALCODE>> GetHospitalCode(string code)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/GetHospitalCode");
            builder.AddOrUpdateParam("code", code);
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<List<HOSPITALCODE>>(responseBody);
            return rspObj;
        }

        public async Task<List<HOSPITALCODELAZY>> GetHospitalCodeLazy(string code)
        {
            var builder = new CustomUriBuilder($"{idoctorAPIUrl}/HealthWeb/GetHospitalCodeLazy");
            builder.AddOrUpdateParam("code", code);
            var url = builder.ToString();
            var response = await client.GetAsyncAndLog(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var rspObj = JsonConvert.DeserializeObject<List<HOSPITALCODELAZY>>(responseBody);
            return rspObj;
        }
    }
}
