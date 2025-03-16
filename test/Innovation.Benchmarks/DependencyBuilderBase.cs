namespace Innovation.Benchmarks
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    public class DependencyBuilderBase
    {
        #region Fields

        private ServiceProvider serviceProvider;

        #endregion Fields

        #region Constructor

        public DependencyBuilderBase()
        {
            this.ConfigureServices();
        }

        #endregion Constructor

        #region Methods

        public T GetRequiredService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }

        #endregion Methods

        #region Private Methods

        private void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(options => { options.SetMinimumLevel(LogLevel.Warning); });
            serviceCollection.AddConsumer();
            serviceCollection.AddInnovation(innovationOptions => { innovationOptions.IsValidationEnabled = true; });

            this.serviceProvider = serviceCollection.BuildServiceProvider();
        }

        #endregion Private Methods
    }
}
