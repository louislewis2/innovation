namespace Innovation.Api.Commanding
{
    using System.Threading.Tasks;

    /// <summary>
    /// This is the interface all command handlers must implement.
    /// </summary>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// This is the method that will handle the issued command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<ICommandResult> Handle(TCommand command);
    }
}
