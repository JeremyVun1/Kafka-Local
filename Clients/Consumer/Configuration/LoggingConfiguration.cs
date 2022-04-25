using Microsoft.Extensions.Hosting;
using Serilog;

namespace Consumer.Configuration;

public static class LoggingConfiguration
{
    public static IHostBuilder ConfigureLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((context, services, config) => {
            config.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();
        });

        return builder;
    }
}