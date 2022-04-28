using Confluent.SchemaRegistry;
using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.Kafka.SyncOverAsync;
using System.Security.Cryptography.X509Certificates;

namespace Producer.Configuration;

public static class KafkaConfiguration
{
    public static IServiceCollection AddSchemaRegistry(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<ISchemaRegistryClient>(provider => {
            var schemaRegistryConfig = new SchemaRegistryConfig();
            config.GetRequiredSection("Kafka:SchemaRegistry").Bind(schemaRegistryConfig);
            return new CachedSchemaRegistryClient(schemaRegistryConfig);
        });

        return services;
    }

    public static IServiceCollection AddProducer<TValue>(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IProducer<string, TValue>>(provider => {
            var producerConfig = new ProducerConfig();
            config.GetRequiredSection("Kafka:Producer").Bind(producerConfig);

            var schemaRegistry = provider.GetRequiredService<ISchemaRegistryClient>();

            return new ProducerBuilder<string, TValue>(producerConfig)
                //.SetKeySerializer(new AvroSerializer<TKey>(schemaRegistry).AsSyncOverAsync())
                .SetValueSerializer(new AvroSerializer<TValue>(schemaRegistry).AsSyncOverAsync())
                .Build();
        });

        return services;
    }
}