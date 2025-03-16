namespace Innovation.SampleApi.Consumer.Handlers
{
    using System.Threading.Tasks;

    using Innovation.ApiSample;
    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    public class BlankCommandPersistor : ICommandHandler<BlankCommand>
    {
        #region Fields

        private static readonly ICommandResult commandResult = new CommandResult();

        #endregion Fields

        public ValueTask<ICommandResult> Handle(BlankCommand command)
        {
            return ValueTask.FromResult(commandResult);
        }
    }
}
