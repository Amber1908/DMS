using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using X1APServer.Service.Model;

namespace WebApplication1.Infrastructure.Common
{
    public class Result
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static Rsp NormalResult<Req, Rsp>(Req request, Rsp response) 
            where Rsp : RSPBase
        {
            return NormalResult(request, response, ErrorCode.OK, "OK");
        }

        public static Rsp NormalResult<Req, Rsp>(Req request, Rsp response, ErrorCode errorCode, string message)
            where Rsp : RSPBase
        {
            response.ReturnCode = errorCode;
            response.ReturnMsg = message;

            //StringBuilder builder = new StringBuilder();
            //builder.Append("\nReceive: \n");
            //builder.Append($"{JsonConvert.SerializeObject(request)} \n");
            //builder.Append("Response: \n");
            //builder.Append($"{JsonConvert.SerializeObject(response)} \n");
            //logger.Debug(builder.ToString());

            return response;
        }

        public static Rsp ErrorResult<Req, Rsp>(Rsp response, Exception e)
            where Rsp : RSPBase
        {
            response.ReturnCode = ErrorCode.ServerError;
            //response.ReturnMsg = "系統發生錯誤";
            response.ReturnMsg = e.Message;

            StringBuilder builder = new StringBuilder();
            builder.Append("Exception: \n");
            builder.Append($"{e.ToString()} \n");
            //builder.Append($"{e.StackTrace} \n");
            logger.Error(builder.ToString());

            return response;
        }

        public static Rsp ErrorResult<Req, Rsp>(Req request, Rsp response, Exception e)
            where Rsp : RSPBase
        {
            response.ReturnCode = ErrorCode.ServerError;
            //response.ReturnMsg = "系統發生錯誤";
            response.ReturnMsg = e.Message;

            StringBuilder builder = new StringBuilder();
            builder.Append("\n Receive: \n");
            builder.Append($"{JsonConvert.SerializeObject(request)} \n");
            builder.Append("Exception: \n");
            builder.Append($"{e.ToString()} \n");
            //builder.Append($"{e.StackTrace} \n");
            logger.Error(builder.ToString());

            return response;
        }
    }
}