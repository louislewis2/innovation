namespace Microsoft.Extensions.DependencyInjection
{
    using Extensions;

    using Innovation.Api.Dispatching;
    using Innovation.ServiceBus.InProcess;
    using Innovation.ServiceBus.InProcess.Dispatching;

    public static class ServiceCollectionExtensions
    {
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