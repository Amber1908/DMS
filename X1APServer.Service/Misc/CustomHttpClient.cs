using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Misc
{
    public class CustomHttpClient : HttpClient
    {
        private Logger logger = LogManager.GetLogger("WebLogger");

        public async Task<HttpResponseMessage> GetAsyncAndLog(string requestUri)
        {
            string requestTime = DateTime.Now.ToString();

            var response = await this.GetAsync(requestUri);

            string receiveTime = DateTime.Now.ToString();
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine();
            logMessage.AppendLine("Request Time: " + requestTime);
            logMessage.AppendLine("Request Uri: " + requestUri);
            logMessage.AppendLine("Response Time: " + DateTime.Now.ToString());
            logMessage.AppendLine("Response Content: " + await response.Content.ReadAsStringAsync());
            logger.Debug(logMessage);

            return response;
        }
    }
}
