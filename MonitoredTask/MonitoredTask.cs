using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandPatternExtension.MonitoredTask
{
    public class MonitoredTask<TParam,TResult>
        : MonitoredTaskBase
        where TParam : class
    {
        public readonly Task<TResult> Task;
        readonly Func<TParam, TResult> _TaskFunction;

        public MonitoredTask(
            string name,
            Func<TParam,TResult> taskFunction,
            TParam taskParameter
            ) : base(name)
        {
            _TaskFunction = taskFunction;
            CancellationToken = new CancellationToken();
            Task = System.Threading.Tasks.Task.Factory.StartNew<TResult>(
                TaskFunctionWrapper,
                (object)taskParameter,
                CancellationToken
                );            
        }

        TResult TaskFunctionWrapper(object parameter)
        {
            StartTime = DateTime.Now;
            if (IsMonitoredTasksExecLogEnabled)
                Log($"monitored task '{Name}' begin exec at {StartTime}");
            var r = _TaskFunction((TParam)parameter);
            EndTime = DateTime.Now;
            if (IsMonitoredTasksExecLogEnabled)
                Log($"monitored task '{Name}' end exec at {EndTime}");
            return r;
        }

    }

    public class MonitoredTask<TParam>
        : MonitoredTaskBase
    {
        readonly Action<TParam> _TaskAction;
        public readonly Task Task;

        public MonitoredTask(
            string name,
            Action<TParam> taskAction,
            TParam taskParameter
            ) : base(name)
        {
            _TaskAction = taskAction;
            CancellationToken = new CancellationToken();
            Task = System.Threading.Tasks.Task.Factory.StartNew(
                TaskActionWrapper,
                taskParameter,
                CancellationToken);
        }

        void TaskActionWrapper(object parameter)
        {
            StartTime = DateTime.Now;
            if (IsMonitoredTasksExecLogEnabled)
                Log($"monitored task '{Name}' begin exec at {StartTime}");
            _TaskAction((TParam)parameter);
            EndTime = DateTime.Now;
            if (IsMonitoredTasksExecLogEnabled)
                Log($"monitored task '{Name}' end exec at {EndTime}");
        }
    }
}
