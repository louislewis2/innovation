namespace Microsoft.Extensions.DependencyInjection
{
    using Innovation.Api.Dispatching;
    using Innovation.SampleApi.Consumer.Stores;

    public static class IServiceCollectionExtensions
    {
        public static void AddConsumer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAuditStore, SampleAuditStore>();
        }
    }
}