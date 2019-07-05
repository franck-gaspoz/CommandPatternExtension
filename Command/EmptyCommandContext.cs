namespace CommandPatternExtension.Command
{
    public class EmptyCommandContext
        : CommandContextBase
    {
        public static EmptyCommandContext Instance
            = new EmptyCommandContext();

        public EmptyCommandContext()
        {
        }
    }
}
