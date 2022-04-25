using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Api.AdminClient.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection AddKafkaServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IAdminClient>((provider) => {
                var config = provider.GetRequiredService<IOptions<AdminClientConfig>>();
                return new AdminClientBuilder(config.Value).Build();
            })
            .AddScoped<IKafkaAdminService, KafkaAdminService>();

        return services;
    }
}