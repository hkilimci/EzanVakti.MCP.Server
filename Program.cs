using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using EzanVakti.MCP.Server.Services;

namespace EzanVakti.MCP.Server;

/// <summary>
/// Main program entry point for the EzanVakti MCP Server
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Configure logging
        builder.Logging.AddConsole(options =>
            options.LogToStandardErrorThreshold = LogLevel.Trace);

        // Register services
        builder.Services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();

        // Register HttpClient for API calls
        builder.Services.AddHttpClient<EzanVaktiApiService>(client =>
        {
            client.BaseAddress = new Uri("https://ezanvakti.emushaf.net");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        await builder.Build().RunAsync();
    }
}





