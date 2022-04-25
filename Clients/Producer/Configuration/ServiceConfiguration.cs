using Models.Kafka.Test;
using Producer.Services;

namespace Producer.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ExampleConfig>(config.GetSection("Example"));

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProducerService, TestProducerService<TestEvent>>();

        return services;
    }
}