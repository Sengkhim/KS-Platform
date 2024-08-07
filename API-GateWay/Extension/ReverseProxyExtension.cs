namespace API_GateWay.Extension;

public static class ReverseProxyExtension
{
    /// <summary>
    /// Adds ReverseProxy's services to Dependency Injection.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    public static void AddReveresProxyService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }
}