namespace Innovation.Sample.Data.Handlers.Customers.Commands
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Data.Anemics;
    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Commands;

    public class CustomerPersistor : ICommandHandler<CreateCustomer>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly ExampleContext context;

        #endregion Fields

        #region Constructor

        public CustomerPersistor(ILogger<CustomerPersistor> logger, ExampleContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        #endregion Constructor

        #region Methods

        public async Task<ICommandResult> Handle(CreateCustomer command)
        {
            return await this.Persist(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(CreateCustomer command)
        {
            var result = new CommandResult();

            try
            {
                var customer = new AnemicCustomer()
                {
                    Id = Guid.NewGuid(),
                    FirstName = command.FirstName,
                    Email = command.Email,
                    LastName = command.LastName,
                    PhoneNumber = command.PhoneNumber,
                    UserName = command.UserName
                };

                context.Customers.Add(customer);
                await context.SaveChangesAsync();

                result.SetRecord(customer.Id);

                return result;
            }
            catch (Exception ex)
            {
                result.Fail(ex);
            }

            return result;
        }

        #endregion Private Methods
    }
}
