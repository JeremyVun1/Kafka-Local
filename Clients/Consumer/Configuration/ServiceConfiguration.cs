using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Consumer.Processors;
using Consumer.WebJobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Kafka.Test;

namespace Consumer.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ConsumerConfig>(config.GetSection("Kafka:Consumer"))
            .Configure<SchemaRegistryConfig>(config.GetSection("Kafka:SchemaRegistry"))
            .Configure<ExampleConfig>(config.GetSection("Example"));

        return services;
    }

    public static IServiceCollection AddJobProcessors(this IServiceCollection services)
    {
        services.AddTransient<IProcessor<Ignore, TestEvent>, ExampleProcessor>()
            .AddSingleton<IJobHost, ExampleJob>();
            

        return services;
    }
}