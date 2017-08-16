namespace Microsoft.Extensions.DependencyInjection
{
    using Innovation.Sample.Data.Contexts;

    public static class IServiceCollectionExtensions
    {
        public static void AddSampleModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ExampleContext>();
        }
    }
}