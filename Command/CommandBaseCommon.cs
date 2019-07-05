using System;

namespace CommandPatternExtension.Command
{
    public class CommandBaseCommon
    {
        #region attributes

        public bool AppBuzy
        {
            get
            {
                return false;
            }
        }

        public virtual event EventHandler CanExecuteChanged;
        /*{
         * // presentationCore.dll
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }*/

        public static Action<string> LogAction;
        public static Action<Exception> LogExceptionAction;
        public static Action<string> LogErrorAction;

        public static bool IsCommandExecLogEnabled = true;

        #endregion
        
    }
}
