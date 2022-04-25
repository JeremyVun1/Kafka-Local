using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Confluent.SchemaRegistry;
using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.Kafka.SyncOverAsync;

namespace Consumer.Configuration;

public static class KafkaConfiguration
{
    public static IServiceCollection AddSchemaRegistry(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<CachedSchemaRegistryClient>(provider => {
            var schemaRegistryConfig = new SchemaRegistryConfig();
            config.GetRequiredSection("Kafka:SchemaRegistry").Bind(schemaRegistryConfig);
            return new CachedSchemaRegistryClient(schemaRegistryConfig);
        });

        return services;
    }

    public static IServiceCollection AddConsumer<TKey, TValue>(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConsumer<TKey, TValue>>(provider => {
            var consumerConfig = new ConsumerConfig();
            config.GetRequiredSection("Kafka:Consumer").Bind(consumerConfig);

            var schemaRegistry = provider.GetRequiredService<CachedSchemaRegistryClient>();

            var builder = new ConsumerBuilder<TKey, TValue>(consumerConfig);

            if (typeof(TKey) != typeof(Ignore))
            {
                builder.SetKeyDeserializer(new AvroDeserializer<TKey>(schemaRegistry).AsSyncOverAsync());
            }

            return builder
                .SetValueDeserializer(new AvroDeserializer<TValue>(schemaRegistry).AsSyncOverAsync())
                .Build();
        });

        return services;
    }
}