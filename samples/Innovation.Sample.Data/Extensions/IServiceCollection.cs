namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.Extensions.Options;
    using Microsoft.EntityFrameworkCore;

    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Infrastructure.Settings;

    public static class IServiceCollectionExtensions
    {
        public static void AddDataModule(this IServiceCollection serviceCollection)
        {
            var temporaryServiceProvider = serviceCollection.BuildServiceProvider();
            var dataBaseSettingsOptions = temporaryServiceProvider.GetRequiredService<IOptions<DataBaseSettings>>();

            serviceCollection.AddScoped<AuditDbContextBase<PrimaryContext>, PrimaryContext>();

            serviceCollection.AddDbContext<PrimaryContext>(options => 
            {
                options.UseInMemoryDatabase(databaseName: dataBaseSettingsOptions.Value.ConnectionString);
            });
        }
    }
}