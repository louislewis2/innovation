namespace Innovation.Api.Interceptors
{
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;

    using Innovation.Api.Commanding;

    public interface ICommandInterceptor<in TCommand> where TCommand : ICommand
    {
        Task Intercept([DisallowNull] TCommand command);
    }
}
