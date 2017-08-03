namespace Innovation.Api.Reactions
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;

    public interface ICommandReactor<in TCommand> where TCommand : ICommand
    {
        Task React(TCommand command);
    }
}
