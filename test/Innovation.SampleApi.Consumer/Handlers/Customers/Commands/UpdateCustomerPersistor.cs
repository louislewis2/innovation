namespace Innovation.SampleApi.Consumer.Handlers.Customers.Commands
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Customers.Commands;

    public class UpdateCustomerPersistor : ICommandHandler<UpdateCustomerCommand>
    {
        #region Methods

        public async ValueTask<ICommandResult> Handle(UpdateCustomerCommand command)
        {
            return await this.Persist(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async ValueTask<ICommandResult> Persist(UpdateCustomerCommand command)
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

            return await Task.FromResult(result: commandResult);
        }

        #endregion Private Methods
    }
}
