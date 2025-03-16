namespace Innovation.Sample.BaseModule.Handlers.Customers.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Commands;

    public class DeleteCustomerCommandPersistor : ICommandHandler<DeleteCustomerCommand>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly PrimaryContext primaryContext;

        #endregion Fields

        #region Constructor

        public DeleteCustomerCommandPersistor(ILogger<DeleteCustomerCommandPersistor> logger, PrimaryContext primaryContext)
        {
            this.logger = logger;
            this.primaryContext = primaryContext;
        }

        #endregion Constructor

        #region Methods

        public async ValueTask<ICommandResult> Handle(DeleteCustomerCommand command)
        {
            return await Delete(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Delete(DeleteCustomerCommand command)
        {
            var commandResult = new CommandResult();

            try
            {
                var customer = primaryContext.Customers
                    .FirstOrDefault(x => x.Id == command.CustomerId);

                if (customer == null)
                {
                    commandResult.Fail(errorMessage: $"A resource with id {command.CustomerId} was not found");

                    return commandResult;
                }

                primaryContext.Customers.Remove(customer);
                await primaryContext.SaveChangesAsync();

                return commandResult;
            }
            catch (Exception ex)
            {
                commandResult.Fail(ex: ex);
            }

            return commandResult;
        }

        #endregion Private Methods
    }
}
