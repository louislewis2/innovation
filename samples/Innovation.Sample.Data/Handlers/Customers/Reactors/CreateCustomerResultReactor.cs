namespace Innovation.Sample.Data.Handlers.Customers.Reactors
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    using Innovation.Api.Reactions;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;

    using Innovation.Sample.Api.Logging.Commands;
    using Innovation.Sample.Api.Customers.Commands;

    public class CreateCustomerResultReactor : ICommandResultReactor<CreateCustomer>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructor

        public CreateCustomerResultReactor(ILogger<CreateCustomerResultReactor> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructor

        #region Methods

        public async Task React(ICommandResult commandResult, CreateCustomer command)
        {
            try
            {
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();

                    var message = $"Create Customer Has Just Fired. Successfully Completed?: { commandResult.Success }";

                    var insertLogEntryCommand = new InsertLogEntry(message: message);
                    var insertLogEntryCommandResult = await dispatcher.Command(command: insertLogEntryCommand);

                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(message: ex.GetInnerMostMessage(), args: ex);
            }
        }

        #endregion Methods
    }
}
