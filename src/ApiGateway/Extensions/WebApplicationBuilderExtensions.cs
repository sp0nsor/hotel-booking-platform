using ApiGateway.Options;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ApiGateway.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void LoadMergedOcelotConfiguration(this WebApplicationBuilder builder)
        {
            var options = builder.Configuration
                .GetSection(nameof(OcelotRoutingOptions))
                .Get<OcelotRoutingOptions>()
                ?? throw new InvalidOperationException("OcelotRoutingOptions configuration missing");

            var routeFiles = Directory.EnumerateFiles(
                options.RoutesDirectory,
                "*.json",
                SearchOption.TopDirectoryOnly);

            var allRoutes = routeFiles
                .Select(File.ReadAllText)
                .Select(JObject.Parse)
                .Where(config => config[options.RoutesKey] is JArray)
                .SelectMany(config => config[options.RoutesKey]!.Children<JObject>())
                .ToList();

            var baseConfig = JObject.Parse(File.ReadAllText(options.BaseConfigPath));
            baseConfig[options.RoutesKey] = new JArray(allRoutes);

            var configString = baseConfig.ToString();
            var configBytes = Encoding.UTF8.GetBytes(configString);
            var memoryStream = new MemoryStream(configBytes);

            builder.Configuration.AddJsonStream(memoryStream);
        }
    }
}
