namespace Innovation.Api.Reactions
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;

    public interface ICommandResultReactor<in TCommand> where TCommand : ICommand
    {
        Task React(ICommandResult commandResult, TCommand command);
    }
}
