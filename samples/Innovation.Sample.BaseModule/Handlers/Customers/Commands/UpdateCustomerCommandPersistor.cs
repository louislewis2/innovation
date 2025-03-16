namespace Innovation.Sample.BaseModule.Handlers.Customers.Commands
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Commands;

    public class UpdateCustomerCommandPersistor : ICommandHandler<UpdateCustomerCommand>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly PrimaryContext primaryContext;

        #endregion Fields

        #region Constructor

        public UpdateCustomerCommandPersistor(ILogger<UpdateCustomerCommandPersistor> logger, PrimaryContext primaryContext)
        {
            this.logger = logger;
            this.primaryContext = primaryContext;
        }

        #endregion Constructor

        #region Methods

        public async ValueTask<ICommandResult> Handle(UpdateCustomerCommand command)
        {
            return await Persist(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(UpdateCustomerCommand command)
        {
            var commandResult = new CommandResult();

            try
            {
                var customer = await primaryContext.Customers
                    .FirstOrDefaultAsync(x => x.Id == command.CustomerId);

                if (customer == null)
                {
                    commandResult.Fail($"Resource with id: {command.CustomerId} could not be found");

                    return commandResult;
                }

                customer.FirstName = command.Criteria.FirstName;
                customer.LastName = command.Criteria.LastName;
                customer.Email = command.Criteria.Email;
                customer.PhoneNumber = command.Criteria.PhoneNumber;

                await primaryContext.SaveChangesAsync();

                commandResult.SetRecord(recordId: customer.Id);

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
