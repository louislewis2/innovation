namespace Innovation.Sample.Web
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;

    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Data.Stores;
    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Infrastructure.Settings;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args: args);

            webApplicationBuilder.Services.Configure<DataBaseSettings>(webApplicationBuilder.Configuration.GetSection(nameof(DataBaseSettings)));
            webApplicationBuilder.Services.Configure<InnovationAuditSettings>(webApplicationBuilder.Configuration.GetSection(nameof(InnovationAuditSettings)));

            webApplicationBuilder.Services.AddBaseModule();
            webApplicationBuilder.Services.AddDataModule();

            // Add Innovation without audit store
            //webApplicationBuilder.Services.AddInnovation();

            // Add innovation with audit store
            webApplicationBuilder.Services.AddInnovation<InnovationAuditStore<PrimaryContext>>();

            webApplicationBuilder.Services
                .AddControllersWithViews()
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            var applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen(swaggerGenOptions =>
            {
                var basePath = AppContext.BaseDirectory;
                var apiXmlPath = Path.Combine(basePath, "Innovation.Sample.Api.xml");
                var webHostXmlPath = Path.Combine(basePath, "Innovation.Sample.Web.xml");

                swaggerGenOptions.IncludeXmlComments(apiXmlPath);
                swaggerGenOptions.IncludeXmlComments(webHostXmlPath);

                swaggerGenOptions.SwaggerDoc(
                    name: applicationVersion,
                    info: new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Innovation Sample Api",
                        Version = applicationVersion,
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact 
                        {
                            Url = new Uri("https://github.com/louislewis2/innovation"),
                            Name = "Innovation Home"
                        }
                    });
            });

            var webApplication = webApplicationBuilder.Build();

            if (webApplication.Environment.IsDevelopment())
            {
                webApplication.UseSwagger();
                webApplication.UseSwaggerUI(swaggerUiOptions =>
                {
                    swaggerUiOptions.SwaggerEndpoint(url: $"/swagger/{applicationVersion}/swagger.json", name: "Innovation Sample Api");
                });

                webApplication.UseDeveloperExceptionPage();
            }
            else
            {
                webApplication.UseExceptionHandler(applicationBuilder =>
                {
                    applicationBuilder.Run(async context =>
                    {
                        var commandResult = new CommandResult();

                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        var defaultMessage = "An error occurred";

                        try
                        {
                            commandResult.Fail(errorMessage: defaultMessage);
                            var responseAsString = JsonSerializer.Serialize(commandResult);

                            await context.Response.WriteAsync(responseAsString);
                        }
                        catch (Exception) // Swallow the exception, as we cannot leak exceptions
                        {
                            await context.Response.WriteAsync(defaultMessage);
                        }
                    });
                });
            }

            webApplication.UseHttpsRedirection();
            webApplication.UseStaticFiles();

            webApplication.UseRouting();

            webApplication.UseAuthentication();
            webApplication.UseAuthorization();

            webApplication.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await webApplication.RunAsync();
        }
    }
}
