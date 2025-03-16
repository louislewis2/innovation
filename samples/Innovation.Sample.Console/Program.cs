namespace Innovation.Sample.Console
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    using Innovation.Api.Dispatching;
    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Api.Paging;
    using Innovation.Sample.Api.Messages;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Api.Customers.Commands;
    using Innovation.Sample.Api.Customers.Criteria;
    using Innovation.Sample.Api.Customers.ViewModels;

    class Program
    {
        #region Methods

        public static async Task Main(string[] args)
        {
            ConfigureServices();
            var dispatcher = ServiceProvider.GetService<IDispatcher>();

            // Command - Exception
            var createCustomerCriteriaInvalid = CreateCustomerCriteria.Default();
            var createCustomerCommandInvalid = new CreateCustomerCommand(createCustomerCriteria: createCustomerCriteriaInvalid);
            var createCustomerCommandInvalidResult = await dispatcher.Command(command: createCustomerCommandInvalid);

            // Command Create
            var createCustomerCriteria = new CreateCustomerCriteria(
                userName: "louislewis2",
                firstName: "Louis",
                lastName: "Lewis",
                email: "louis@domainnotexisting.org",
                phoneNumber: "5555555");

            var createCustomerCommand = new CreateCustomerCommand(createCustomerCriteria: createCustomerCriteria);
            var createCustomerCommandResult = await dispatcher.Command(command: createCustomerCommand);
            var createdCustomerId = Guid.Parse(((CommandResult)createCustomerCommandResult).RecordId);

            // Query Paged
            var queryPage = new QueryPage();
            var queryPagedResult = await dispatcher.Query<QueryPage, GenericResultsList<CustomerLite>>(query: queryPage);

            // Query Single Lite
            var getCustomerQuery = new GetCustomerQuery(customerId: createdCustomerId);
            var customerLite = await dispatcher.Query<GetCustomerQuery, CustomerLite>(getCustomerQuery);

            // Query Single Detail
            var customerDetail = await dispatcher.Query<GetCustomerQuery, CustomerDetail>(query: getCustomerQuery);

            // Command Delete
            var deleteCommand = new DeleteCustomerCommand(customerId: createdCustomerId);
            var deleteResult = await dispatcher.Command(command: deleteCommand);

            // Message
            var exception = new NotImplementedException("Lets throw an exception");
            var exceptionMessage = new ExceptionMessage { Exception = exception };
            await dispatcher.Message(exceptionMessage);
        }

        #endregion Methods

        #region Properties

        public static IServiceProvider ServiceProvider { get; set; }

        #endregion Properties

        #region Private Methods

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddDebug().SetMinimumLevel(LogLevel.Information);
                config.AddConsole();
            });

            services.AddOptions();

            services.AddBaseModule();
            services.AddDataModule();
            services.AddInnovation();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        #endregion Private Methods
    }
}