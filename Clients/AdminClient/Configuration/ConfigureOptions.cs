using Confluent.Kafka;

namespace Api.AdminClient.Configuration;

public static class ConfigureOptions
{
    public static IServiceCollection BindOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AdminClientConfig>(config.GetSection("KafkaAdminClient"));

        return services;
    }
}