namespace Innovation.Sample.Data.Handlers.Customers.Reactors
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Reactions;
    using Innovation.Api.Commanding;

    using Innovation.Sample.Api.Customers.Commands;

    public class CreateCustomerResultReactor : ICommandResultReactor<CreateCustomer>
    {
        #region Fields

        private readonly ILogger logger;

        #endregion Fields

        #region Constructor

        public CreateCustomerResultReactor(ILogger<CreateCustomerResultReactor> logger)
        {
            this.logger = logger;
        }

        #endregion Constructor

        #region Methods

        public async Task React(ICommandResult commandResult, CreateCustomer command)
        {
            try
            {
                await Task.Run(() =>
                {
                    Console.WriteLine($"Create Customer Has Just Fired. Successfully Completed?: { commandResult.Success }");
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(message: ex.GetInnerMostMessage(), args: ex);
            }
        }

        #endregion Methods
    }
}
