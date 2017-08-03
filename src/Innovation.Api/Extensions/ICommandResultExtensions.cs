namespace Innovation.Api.Commanding
{
    public static class ICommandResultExtensions
    {
        public static T As<T>(this ICommandResult commandResult)
        {
            return (T)commandResult;
        }
    }
}