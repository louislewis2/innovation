namespace Innovation.Sample.BaseModule.Handlers.Customers.Reactors
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    using Innovation.Api.Reactions;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;

    using Innovation.Sample.Api.Logging.Criteria;
    using Innovation.Sample.Api.Logging.Commands;
    using Innovation.Sample.Api.Customers.Commands;

    public class CreateCustomerCommandResultReactor : ICommandResultReactor<CreateCustomerCommand>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructor

        public CreateCustomerCommandResultReactor(ILogger<CreateCustomerCommandResultReactor> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructor

        #region Methods

        public async Task React(ICommandResult commandResult, CreateCustomerCommand command)
        {
            try
            {
                // Simulate A Long Running Task
                Thread.Sleep(5000);

                using (var scope = serviceProvider.CreateScope())
                {
                    var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();

                    var message = $"Create Customer Has Just Fired. Successfully Completed?: {commandResult.Success}";

                    var insertLogEntryCriteria = new InsertLogEntryCriteria(message: message);
                    var insertLogEntryCommand = new InsertLogEntryCommand(insertLogEntryCriteria: insertLogEntryCriteria);
                    var insertLogEntryCommandResult = await dispatcher.Command(command: insertLogEntryCommand);

                    this.logger.LogInformation(message: message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(exception: ex, message: ex.GetInnerMostMessage());
            }
        }

        #endregion Methods
    }
}
