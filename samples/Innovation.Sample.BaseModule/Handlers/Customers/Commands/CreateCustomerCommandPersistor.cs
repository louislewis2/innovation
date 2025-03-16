namespace Innovation.Sample.BaseModule.Handlers.Customers.Commands
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Commands;
    using Innovation.Sample.Data.Anemics.Customers;

    public class CreateCustomerCommandPersistor : ICommandHandler<CreateCustomerCommand>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly PrimaryContext primarycontext;

        #endregion Fields

        #region Constructor

        public CreateCustomerCommandPersistor(ILogger<CreateCustomerCommandPersistor> logger, PrimaryContext primarycontext)
        {
            this.logger = logger;
            this.primarycontext = primarycontext;
        }

        #endregion Constructor

        #region Methods

        public async ValueTask<ICommandResult> Handle(CreateCustomerCommand command)
        {
            return await Persist(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(CreateCustomerCommand command)
        {
            var result = new CommandResult();

            try
            {
                var customer = CustomerAnemic.New(
                    id: Guid.NewGuid(),
                    userName: command.Criteria.UserName,
                    firstName: command.Criteria.FirstName,
                    email: command.Criteria.Email,
                    lastName: command.Criteria.LastName,
                    phoneNumber: command.Criteria.PhoneNumber);

                primarycontext.Customers.Add(entity: customer);
                await primarycontext.SaveChangesAsync();

                result.SetRecord(recordId: customer.Id);

                return result;
            }
            catch (Exception ex)
            {
                result.Fail(ex: ex);
            }

            return result;
        }

        #endregion Private Methods
    }
}
