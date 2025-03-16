namespace Innovation.Api.Reactions
{
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;

    using Innovation.Api.Commanding;

    public interface ICommandReactor<in TCommand> where TCommand : ICommand
    {
        Task React([DisallowNull] TCommand command);
    }
}
