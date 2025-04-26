using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace UserService.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder UseElk(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, config) =>
            {
                var environment = context.HostingEnvironment.EnvironmentName;
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

                config
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Environment", environment)
                    .Enrich.WithProperty("Application", assemblyName)
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat = $"{assemblyName?.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}"
                    });
            });

            return builder;
        }
    }
}
