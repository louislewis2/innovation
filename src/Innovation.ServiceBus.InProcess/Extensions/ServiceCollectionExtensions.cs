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
        /// </summary>
        /// <example>services.AddInnovation();</example>
        public static void AddInnovation(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddInnovation(innovationOptions => { innovationOptions.IsValidationEnabled = true; });
        }

        /// <summary>
        /// This Will Add All The Required Innovation Components and register options
        /// </summary>
        /// <example>services.AddInnovation();</example>
        public static void AddInnovation(this IServiceCollection serviceCollection, Action<InnovationOptions> innovationOptions)
        {
            serviceCollection.Configure(innovationOptions);
            var provider = serviceCollection.BuildServiceProvider();

            var runtime = ActivatorUtilities.CreateInstance<InnovationRuntime>(provider, serviceCollection);
            serviceCollection.TryAddSingleton(runtime);

            runtime.Configure();
            serviceCollection.TryAddTransient<IDispatcher, Dispatcher>();
        }
    }
}