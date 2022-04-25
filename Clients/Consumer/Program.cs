using Confluent.Kafka;
using Consumer.Configuration;
using Consumer.WebJobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models.Kafka.Test;

namespace Consumer;

public class Program
{
    public static async Task Main()
    {
        var host = new HostBuilder()
            .ConfigureWebJobs()
            .ConfigureAppConfiguration((context, builder) => {
                context.Configuration = builder
                    .SetBasePath(context.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile("appsettings.secret.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            })
            .ConfigureLogging()
            .ConfigureServices((context, services) => {
                var config = context.Configuration;

                services
                    .BindConfigurations(config)
                    .AddHttpClients(config)
                    .AddSchemaRegistry(config)
                    .AddConsumer<Ignore, TestEvent>(config)
                    .AddJobProcessors();
            })
            .Build();

        await Run(host);
    }

    public static async Task Run(IHost host)
    {
        var config = host.Services.GetService(typeof(IConfiguration)) as IConfiguration;

        // WebJobs token watches WEBJOBS_SHUTDOWN_FILE controlled by the App Host
        var ct = new WebJobsShutdownWatcher().Token;

        using (host)
        {
            if (host.Services.GetService(typeof(IJobHost)) is ExampleJob jobHost)
            {
                await host.RunAsync(ct);
            }
        }
    }
}