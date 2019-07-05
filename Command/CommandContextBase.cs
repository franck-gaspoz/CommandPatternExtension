using CommandPatternExtension.MonitoredTask;
using ModelExtensions;
using System;

namespace CommandPatternExtension.Command
{
    public class CommandContextBase
        : BindableModel
    {
        #region attributs

        public CommandContextBase ParentContext;
        public CommandBaseCommon CommandBaseCommon;
        public long Id;

        public DateTime StartTime;
        public DateTime EndTime;
        public Exception Exception;

        public bool RunAsParallelTask = false;
        public bool WaitTaskCompleted = true;

        public MonitoredTaskBase MonitoredTask;

        public Action<string> LogAction;
        public Action<Exception> LogExceptionAction;
        public Action<string> LogErrorAction;

        static long _Id;
        static object _IdLock = new object();

        #endregion

        public CommandContextBase() {
            Init();
        }

        public CommandContextBase(
            bool runAsParallelTask, 
            bool waitTaskCompleted)
        {
            RunAsParallelTask = runAsParallelTask;
            WaitTaskCompleted = waitTaskCompleted;
            Init();
        }

        void Init()
        {
            lock (_IdLock)
            {
                Id = _Id;
                _Id++;
            }
        }

        /// <summary>
        /// get a dump of the last command result if any, otherwise returns null
        /// </summary>
        /// <returns>dump of result if any else null</returns>
        public virtual string DumpResult()
        {
            return null;
        }

        /// <summary>
        /// reset the context before an associable command is executed. may cleanup any previous result if the command context is reused and/or shared
        /// </summary>
        public virtual void Prepare()
        {

        }

        #region log

        public void Log(string s)
        {
            if (LogAction == null)
                CommandBaseCommon.LogAction?.Invoke(s);
            else
                LogAction?.Invoke(s);
        }

        public void LogError(string s)
        {
            if (LogErrorAction == null)
                CommandBaseCommon.LogErrorAction?.Invoke(s);
            else
                LogErrorAction?.Invoke(s);
        }

        public void LogError(Exception ex)
        {
            if (LogExceptionAction == null)
                CommandBaseCommon.LogExceptionAction?.Invoke(ex);
            else
                LogExceptionAction?.Invoke(ex);
        }

        #endregion
    }
}
