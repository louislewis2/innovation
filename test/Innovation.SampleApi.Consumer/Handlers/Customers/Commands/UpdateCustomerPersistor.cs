namespace Innovation.SampleApi.Consumer.Handlers.Customers.Commands
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Customers.Commands;

    public class UpdateCustomerPersistor : ICommandHandler<UpdateCustomer>
    {
        #region Methods

        public async Task<ICommandResult> Handle(UpdateCustomer command)
        {
            return await this.Persist(command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(UpdateCustomer command)
        {
            var commandResult = new CommandResult();

            if (command.DispatcherContext == null)
            {
                commandResult.Fail(errorMessage: "Dispatcher Context Not Set");
            }
            else
            {
                commandResult.SetRecord(recordId: command.CustomerId);
            }

            return await Task.FromResult(commandResult);
        }

        #endregion Private Methods
    }
}
