using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;

namespace WebApplication1.Infrastructure.Common
{
    public class LogMessageHandler : MessageProcessingHandler
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.Info(request.Content.ReadAsStringAsync().Result.ToString());

            return request;
        }

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return response;
        }
    }
}