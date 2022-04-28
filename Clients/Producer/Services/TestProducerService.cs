using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Producer.Models;
using Producer.Configuration;

namespace Producer.Services;

public interface IProducerService
{
    Task<bool> Produce(ProduceRequest request);
}

public class TestProducerService<TValue> : IProducerService
{
    private readonly IProducer<string, TValue> _producer;
    private readonly ISchemaRegistryClient _registry;
    private readonly ILogger<TestProducerService<TValue>> _logger;
    private readonly ExampleConfig _config;

    public TestProducerService(
        IProducer<string, TValue> producer,
        ISchemaRegistryClient registry,
        ILogger<TestProducerService<TValue>> logger,
        IOptions<ExampleConfig> config
    )
    {
        _producer = producer;
        _registry = registry;
        _config = config.Value;
        _logger = logger;
    }

    public async Task<bool> Produce(ProduceRequest request)
    {
        var topicName = _config.Topic;

        _logger.LogInformation("{topic} - {type} - {value}", topicName, typeof(TValue), request.Value);
        
        var obj = JsonConvert.DeserializeObject<TValue>(request.Value);
        _logger.LogInformation("{obj}", obj);

        try
        {
            var result = await _producer.ProduceAsync(topicName, new Message<string, TValue>()
            {
                Key = Guid.NewGuid().ToString(),
                Value = obj
            });

            _logger.LogInformation("status: {status}", result.Status);
            return result.Status == PersistenceStatus.Persisted;
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex);
            return false;
        }
    }
}