namespace CommandPatternExtension.Command
{
    public static class CommandExts
    {
        public static EmptyCommandContext EmptyCommandContext { get; }
            = new EmptyCommandContext();

        public static EmptyCommandContext EmptyCommandParrallelContext { get; }
            = new EmptyCommandContext() { RunAsParallelTask = true };
    }
}
