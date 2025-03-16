namespace Innovation.Sample.BaseModule.Handlers.Customers.Reactors
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Reactions;

    using Innovation.Sample.Api.Customers.Commands;

    public class CreateCustomerCommandReactor : ICommandReactor<CreateCustomerCommand>
    {
        #region Fields

        private readonly ILogger logger;

        #endregion Fields

        #region Constructor

        public CreateCustomerCommandReactor(ILogger<CreateCustomerCommandReactor> logger)
        {
            this.logger = logger;
        }

        #endregion Constructor

        public async Task React(CreateCustomerCommand command)
        {
            await Task.Run(() =>
            {
                this.logger.LogInformation($"Create Customer Event About To Fire: {command.Criteria.FirstName}");
            });
        }
    }
}
