namespace Innovation.ServiceBus.InProcess.Tests
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    using Innovation.Api.Dispatching;

    public class TestBase
    {
        #region Constructor

        public TestBase()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddOptions();

            services.AddInnovation();
            services.AddConsumer();

            this.ServiceProvider = services.BuildServiceProvider();

            var loggerFactory = this.ServiceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddDebug(LogLevel.Debug);
            loggerFactory.AddConsole(LogLevel.Debug);
        }

        #endregion Constructor

        #region Properties

        public IServiceProvider ServiceProvider { get; private set; }

        #endregion Properties

        #region Methods

        internal TSource GetService<TSource>()
        {
            return this.ServiceProvider.GetRequiredService<TSource>();
        }

        internal IDispatcher GetDispatcher()
        {
            return this.GetService<IDispatcher>();
        }

        #endregion Methods
    }
}
