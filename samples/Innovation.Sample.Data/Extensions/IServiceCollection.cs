namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.EntityFrameworkCore.Infrastructure;

    using Innovation.Sample.Data.Contexts;

    public static class IServiceCollectionExtensions
    {
        public static void AddSampleModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddEntityFramework()
                .AddDbContext<ExampleContext>();
        }
    }
}