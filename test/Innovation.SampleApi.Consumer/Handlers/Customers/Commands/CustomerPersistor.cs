namespace Innovation.SampleApi.Consumer.Handlers.Customers.Commands
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Customers.Commands;

    public class CustomerPersistor : ICommandHandler<InsertCustomer>
    {
        #region Methods

        public async Task<ICommandResult> Handle(InsertCustomer command)
        {
            return await this.Persist(command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(InsertCustomer command)
        {
            return await Task.FromResult(new CommandResult());
        }

        #endregion Private Methods
    }
}
