namespace Innovation.SampleApi.Consumer.Handlers.Customers.Commands
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Customers.Commands;

    public class InsertCustomerPersistor : ICommandHandler<InsertCustomerCommand>
    {
        #region Methods

        public async ValueTask<ICommandResult> Handle(InsertCustomerCommand command)
        {
            return await this.Persist(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(InsertCustomerCommand command)
        {
            return await Task.FromResult(result: new CommandResult());
        }

        #endregion Private Methods
    }
}
