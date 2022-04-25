using Serilog;

namespace Producer.Configuration;

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