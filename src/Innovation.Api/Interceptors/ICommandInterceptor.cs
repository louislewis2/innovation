namespace Innovation.Api.Interceptors
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;

    public interface ICommandInterceptor<in TCommand> where TCommand : ICommand
    {
        Task Intercept(TCommand command);
    }
}
