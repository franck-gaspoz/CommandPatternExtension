using CommandPatternExtension.MonitoredTask;
using System;
using System.Windows.Input;

namespace CommandPatternExtension.Command
{
    public abstract class CommandBase<T>
        : CommandBaseCommon, 
        ICommand
        where T : CommandContextBase
    {
        #region interface ICommand

        public bool CanExecute(object commandContext)
        {
            if (commandContext == null)
                return false;
            if (commandContext is T t)
                return !AppBuzy && CanExecuteImpl(t);
            else
                return false;
        }

        protected virtual bool CanExecuteImpl(T commandContext)
        {
            return true;
        }

        public void Execute(object commandContext)
        {
            if (CanExecute(commandContext))
            {
                if (commandContext is T t)
                {
                    try
                    {
                        t.Prepare();
                        t.StartTime = DateTime.Now;
                        t.CommandBaseCommon = this;
                        if (IsCommandExecLogEnabled)
                            t.Log($"start command '{GetType().Name}' at {t.StartTime}");

                        if (!t.RunAsParallelTask)
                        {
                            ExecuteImpl(t);
                            t.EndTime = DateTime.Now;
                            if (IsCommandExecLogEnabled)
                                t.Log($"command '{GetType().Name}' ended at {t.EndTime}");
                        }
                        else
                        {
                            var task =
                                new MonitoredTask<T>(
                                    GetType().Name,
                                    ExecuteImpl,
                                    t
                                    );
                            t.MonitoredTask = task;
                            if (t.WaitTaskCompleted)
                            {
                                task.Task.Wait();
                                t.EndTime = DateTime.Now;
                                if (IsCommandExecLogEnabled)
                                    t.Log($"command '{GetType().Name}' ended at {t.EndTime}");
                            }
                        }
                    } catch (Exception ex)
                    {
                        t.Exception = ex;
                        throw;
                    }
                }
                else
                   throw new Exception($"wrong command context type: {commandContext}");
            }
        }

        protected abstract void ExecuteImpl(T commandContext);

        #endregion                
    }
}
