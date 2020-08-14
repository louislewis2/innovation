namespace Microsoft.Extensions.DependencyInjection
{
    using System;

    using Innovation.Integration.AspNetCore;

    public static class IServiceCollectionExtensions
    {
        public static void AddInnovationAspNetIntegration(this IServiceCollection serviceCollection)
            => serviceCollection.AddInnovationAspNetIntegration(correlationIdOptions => { });

        public static void AddInnovationAspNetIntegration(this IServiceCollection serviceCollection, string correlationIdHeaderName)
            => serviceCollection.AddInnovationAspNetIntegration(correlationIdOptions => { correlationIdOptions.Header = correlationIdHeaderName; });

        public static void AddInnovationAspNetIntegration(this IServiceCollection serviceCollection, Action<CorrelationIdOptions> configureOptions)
        {
            serviceCollection.Configure(configureOptions);
        }
    }
}
