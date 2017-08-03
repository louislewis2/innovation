namespace Innovation.Sample.Data.Handlers.Customers.Commands
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;
    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Commands;

    public class CustomerUpdater : ICommandHandler<UpdateCustomer>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly ExampleContext context;

        #endregion Fields

        #region Constructor

        public CustomerUpdater(ILogger<CustomerUpdater> logger, ExampleContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        #endregion Constructor

        #region Methods

        public async Task<ICommandResult> Handle(UpdateCustomer command)
        {
            return await this.Persist(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(UpdateCustomer command)
        {
            var result = new CommandResult();

            try
            {
                var customer = await this.context.Customers.FirstAsync(x => x.Id == command.Id);

                customer.FirstName = command.FirstName;
                customer.LastName = command.LastName;
                customer.Email = command.Email;
                customer.PhoneNumber = command.Phone;

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
