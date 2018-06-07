namespace Innovation.Sample.Data.Handlers.Customers.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Commands;

    public class CustomerDeleter : ICommandHandler<DeleteCustomer>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly ExampleContext context;

        #endregion Fields

        #region Constructor

        public CustomerDeleter(ILogger<CustomerDeleter> logger, ExampleContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        #endregion Constructor

        #region Methods

        public async Task<ICommandResult> Handle(DeleteCustomer command)
        {
            return await this.Delete(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Delete(DeleteCustomer command)
        {
            var result = new CommandResult();

            try
            {
                var customer = this.context.Customers.FirstOrDefault(x => x.Id == command.Id);

                if (customer == null)
                {
                    throw new ArgumentException($"A resource with id {command.Id} was not found");
                }

                context.Customers.Remove(customer);
                await context.SaveChangesAsync();

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
