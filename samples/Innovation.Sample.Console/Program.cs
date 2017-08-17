namespace Innovation.Sample.Console
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    using Innovation.Api.Dispatching;
    using Innovation.Sample.Api.Paging;
    using Innovation.Api.CommandHelpers;
    using Innovation.Sample.Api.Messages;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Api.Customers.Commands;
    using Innovation.Sample.Api.Customers.ViewModels;

    class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            ConfigureServices();
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args)
        {
            var dispatcher = ServiceProvider.GetService<IDispatcher>();

            // Command - Exception
            var createFaultedCustomerCommand = new CreateCustomer();
            var faultedResult = await dispatcher.Command(createFaultedCustomerCommand);

            // Command Create
            var createCustomerCommand = new CreateCustomer();
            createCustomerCommand.FirstName = "Louis";
            createCustomerCommand.LastName = "Lewis";
            createCustomerCommand.UserName = "louis@lewisonline.co.za";

            var createResult = await dispatcher.Command(createCustomerCommand);
            var createdCustomerId = Guid.Parse(((CommandResult)createResult).RecordId);

            // Query Paged
            var queryPage = new QueryPage();
            var queryPagedResult = await dispatcher.Query<QueryPage, GenericResultsList<CustomerLite>>(queryPage);

            // Query Single Lite
            var getCustomerQuery = new GetCustomer(id: createdCustomerId);
            var customerLite = await dispatcher.Query<GetCustomer, CustomerLite>(getCustomerQuery);

            // Query Single Detail
            var customerDetail = await dispatcher.Query<GetCustomer, CustomerDetail>(getCustomerQuery);

            // Command Delete
            var deleteCommand = new DeleteCustomer(id: createdCustomerId);
            var deleteResult = await dispatcher.Command(deleteCommand);

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
            services.AddLogging();
            services.AddOptions();

            services.AddSampleModule();
            services.AddInnovation();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var loggerFactory = ServiceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddDebug(LogLevel.Debug);
            loggerFactory.AddConsole(LogLevel.Debug);
        }

        #endregion Private Methods
    }
}