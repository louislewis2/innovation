namespace Innovation.SampleApi.Consumer.Handlers.Customers.Reactors
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Reactions;
    using Innovation.Api.Commanding;

    using Innovation.ApiSample.Customers.Commands;
    using Microsoft.Extensions.DependencyInjection;
    using Innovation.Api.Messaging;

    public class InsertCustomerCommandResultReactor : ICommandResultReactor<InsertCustomerCommand>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructor

        public InsertCustomerCommandResultReactor(ILogger<InsertCustomerCommandResultReactor> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructor

        #region Methods

        public Task React(ICommandResult commandResult, InsertCustomerCommand command)
        {
            try
            {
                using (var scope = this.serviceProvider.CreateScope())
                {
                    this.logger.LogInformation(message: "Retrieving services from scope");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(exception: ex, message: ex.GetInnerMostMessage());
            }

            return Task.CompletedTask;
        }

        #endregion Methods
    }
}
