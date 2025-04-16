namespace ApiGateway.Options
{
    public class OcelotRoutingOptions
    {
        public string BaseConfigPath { get; set; } = string.Empty;
        public string RoutesDirectory { get; set; } = string.Empty;
        public string RoutesKey { get; set; } = string.Empty;
    }
}
