namespace Innovation.Api.Reactions
{
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;

    using Innovation.Api.Commanding;

    public interface ICommandResultReactor<in TCommand> where TCommand : ICommand
    {
        Task React([DisallowNull] ICommandResult commandResult, [DisallowNull] TCommand command);
    }
}
