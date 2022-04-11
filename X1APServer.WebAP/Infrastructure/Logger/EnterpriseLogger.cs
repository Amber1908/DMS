//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace X1APServer.Infrastructure.Logger
{
    /// <summary>
    /// 使用Microsoft.Practices.EnterpriseLibrary的記錄器
    /// </summary>
    //public class EnterpriseLogger : ILogger
    //{
    //    public string Category { get; set; }

    //    public EnterpriseLogger()
    //    {
    //        Category = string.Empty;
    //    }

    //    public void Write(string message, TraceEventType severity)
    //    {
    //        LogEntry entry = new LogEntry()
    //        {
    //            Categories = new string[] { Category },
    //            Message = message,
    //            Severity = severity
    //        };

    //        Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(entry);
    //    }

    //    public void Critical(string message)
    //    {
    //        Write(message, TraceEventType.Critical);
    //    }

    //    public void Error(string message)
    //    {
    //        Write(message, TraceEventType.Error);
    //    }

    //    public void Warning(string message)
    //    {
    //        Write(message, TraceEventType.Warning);
    //    }

    //    public void Information(string message)
    //    {
    //        Write(message, TraceEventType.Information);
    //    }
    //}
}