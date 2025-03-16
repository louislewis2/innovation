namespace Innovation.Api.Commanding
{
    public static class ICommandResultExtensions
    {
        public static TCommandResultType As<TCommandResultType>(this ICommandResult commandResult)
        {
            return (TCommandResultType)commandResult;
        }
    }
}