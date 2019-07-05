using System;
using System.Threading;

namespace CommandPatternExtension.MonitoredTask
{
    public class MonitoredTaskBase
    {
        public CancellationToken CancellationToken;
        public DateTime StartTime;
        public DateTime EndTime;
        public string Name;

        public static Action<string> LogAction;
        public static Action<Exception> LogException;
        public static Action<string> LogError;

        public static bool IsMonitoredTasksExecLogEnabled = true;

        //public event EventHandler<MonitoredTaskBase> Exited;        

        public MonitoredTaskBase(string name)
        {
            Name = name;
        }

        #region log

        public void Log(string s)
        {
            LogAction?.Invoke(s);
        }

        public void Error(string s)
        {
            LogError?.Invoke(s);
        }

        public void Error(Exception ex)
        {
            LogException?.Invoke(ex);
        }

        #endregion
    }
}
