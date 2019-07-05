using System.Collections.Generic;
using System.Windows.Input;
using System;

namespace CommandPatternExtension.Command
{
    public class MultiCommand
        : CommandBase<MultiCommandContext>
    {
        List<ICommand> _Commands
            = new List<ICommand>();

        public MultiCommand(IList<ICommand> commands)
        {
            _Commands.AddRange(commands);
        }

        protected override bool CanExecuteImpl(MultiCommandContext commandContext)
        {
            bool canExecute = true;
            int i = 0;
            foreach (var cmd in _Commands)
            {
                canExecute &= cmd.CanExecute(commandContext.CommandContexts[i]);
                if (!canExecute)
                    break;
                i++;
            }
            return canExecute;
        }

        protected override void ExecuteImpl(MultiCommandContext commandContext)
        {
            int i = 0;
            foreach ( var cmd in _Commands )
            {
                if (i > commandContext.CommandContexts.Count)
                    throw new InvalidOperationException($"commandContext[{i}] is not defined");
                cmd.Execute(commandContext.CommandContexts[i]);
                i++;
            }
        }
    }

    public class MultiCommandContext
        : CommandContextBase
    {        
        public List<CommandContextBase> CommandContexts
            = new List<CommandContextBase>();

        public MultiCommandContext(IList<CommandContextBase> commandContexts)
        {
            CommandContexts.AddRange(commandContexts);
            RunAsParallelTask = false;
        }
    }
}
