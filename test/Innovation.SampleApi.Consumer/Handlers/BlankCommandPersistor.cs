namespace Innovation.SampleApi.Consumer.Handlers
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using Innovation.ApiSample;

    public class BlankCommandPersistor : ICommandHandler<BlankCommand>
    {
        #region Fields

        private static readonly ICommandResult commandResult = new CommandResult();

        #endregion Fields

        #region Methods

        public Task<ICommandResult> Handle(BlankCommand command)
        {
            return Task.FromResult(commandResult);
        }

        #endregion Methods
    }
}
