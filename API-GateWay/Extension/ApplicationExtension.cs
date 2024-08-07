namespace API_GateWay.Extension;

public  static class ApplicationExtension
{
    /// <summary>
    /// Adds Reverse Proxy routes to the route table using the default processing pipeline.
    /// </summary>
    /// <param name="app"></param>
    public static void AddMapReverseProxy(this WebApplication app)
    {
        app.MapReverseProxy();
    }

    public static void AddBathConfiguration(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapControllers();
    }
}