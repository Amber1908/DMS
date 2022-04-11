using Newtonsoft.Json;
using NLog;
using System.Threading.Tasks;

namespace X1APServer.Repository.Utility
{
    /// <summary>
    /// Used for the Status property of Result class.
    /// </summary>
    public enum ResultStatus
    {
        Error = -1,
        Ok = 0,
        DataExisted = 1,
        DataNotExisted = 2,
        TokenInvalid = 9,
        ApiTokenInvalid = 99
    }

    /// <summary>
    /// API Result class.
    /// </summary>
    public class Result
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Create and return Result with Error status and given message.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <returns></returns>
        public static Result ErrorResult(string message)
        {
            return ErrorResult(ResultStatus.Error, message);
        }

        /// <summary>
        /// Create and return Result with Error status and given message.
        /// </summary>
        /// <param name="statusCode">ResultStatus</param>
        /// <param name="message">Error message</param>
        /// <returns></returns>
        public static Result ErrorResult(ResultStatus statusCode, string message)
        {
            var result = new Result
            {
                Status = statusCode,
                Message = message
            };
            logger.Error($"Retuen to Client : \n {JsonConvert.SerializeObject(result)}");
            return result;
        }

        public static Result NormalResult(object data, string message = "")
        {
            var result = new Result
            {
                Status = ResultStatus.Ok,
                Message = message,
                Data = data
            };
            logger.Debug($"Retuen to Client : \n {JsonConvert.SerializeObject(result)}");
            return result;
        }

        public static TaskCompletionSource<Result> ErrorTaskCompletionSource(string message)
        {
            var tcs = new TaskCompletionSource<Result>();
            tcs.SetResult(ErrorResult(message));
            return tcs;
        }

        public static TaskCompletionSource<Result> ErrorTaskCompletionSource(ResultStatus statusCode, string message)
        {
            var tcs = new TaskCompletionSource<Result>();
            tcs.SetResult(ErrorResult(statusCode, message));
            return tcs;
        }

        public static TaskCompletionSource<Result> NormalTaskCompletionSource(object data, string message = "")
        {
            var tcs = new TaskCompletionSource<Result>();
            tcs.SetResult(NormalResult(data, message));
            return tcs;
        }

        public static Task<Result> ErrorTask(string message)
        {
            return ErrorTaskCompletionSource(message).Task;
        }

        public static Task<Result> ErrorTask(ResultStatus statusCode, string message)
        {
            return ErrorTaskCompletionSource(statusCode, message).Task;
        }

        public static Task<Result> NormalTask(object data, string message = "")
        {
            return NormalTaskCompletionSource(data, message).Task;
        }

        public object Data { get; set; }
        public string Message { get; set; }
        public ResultStatus Status { get; set; }

    }
}
