namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Extensions;

    using Innovation.Api.Dispatching;
    using Innovation.ServiceBus.InProcess;
    using Innovation.ServiceBus.InProcess.Settings;
    using Innovation.ServiceBus.InProcess.Dispatching;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// This Will Add All The Required Innovation Components
        /// It Also Allows Registering An Implementation Of The Audit Store Interface
        /// </summary>
        /// <example>services.AddInnovation<MyAuditStore>();</example>
        public static void AddInnovation<IAuditHandler>(this IServiceCollection serviceCollection) where IAuditHandler : class, IAuditStore
        {
            serviceCollection.TryAddTransient<IAuditStore, IAuditHandler>();
            serviceCollection.AddInnovation();
        }

        /// <summary>
        /// This Will Add All The Required Innovation Components
        /// It Also Will Expose An Action For Registering Search Locations.
        /// Search locations can contain dll's which will be dynamically loaded and processed
        /// </summary>
        /// <example>
        /// services.AddInnovation(searchLocationsOptions: (searchLocationsOptions) =>
        /// {
        ///   searchLocationsOptions.Locations = new List<string> { "/modules" };
        /// });
        /// </example>
        public static void AddInnovation(this IServiceCollection serviceCollection, Action<SearchLocations> searchLocationsOptions)
        {
            serviceCollection.Configure(searchLocationsOptions);
            serviceCollection.AddInnovation();
        }

        /// <summary>
        /// This Will Add All The Required Innovation Components
        /// </summary>
        /// <example>services.AddInnovation();</example>
        public static void AddInnovation(this IServiceCollection serviceCollection)
        {
            var provider = serviceCollection.BuildServiceProvider();

            var runtime = ActivatorUtilities.CreateInstance<InnovationRuntime>(provider, serviceCollection);
            serviceCollection.TryAddSingleton(runtime);

            runtime.Configure();
            serviceCollection.TryAddTransient<IDispatcher, Dispatcher>();
        }
    }
}