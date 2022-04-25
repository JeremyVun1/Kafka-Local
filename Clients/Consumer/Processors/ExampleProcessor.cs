using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Consumer.Configuration;
using Consumer.Extensions;
using Consumer.Proxies;
using Models.Kafka.Test;

namespace Consumer.Processors;

public class ExampleProcessor : IProcessor<Ignore, TestEvent>
{
    private readonly ExampleConfig _config;
    private readonly IExampleProxy _proxy;
    private readonly ILogger<ExampleProcessor> _logger;

    public ExampleProcessor(
        IOptions<ExampleConfig> config,
        IExampleProxy proxy,
        ILogger<ExampleProcessor> logger
    )
    {
        _config = config.Value;
        _proxy = proxy;
        _logger = logger;
    }

    public async Task<object> Process(ConsumeResult<Ignore, TestEvent> record)
    {
        _logger.LogEventInformation("Filtering", record);
        if (IsValid(record))
        {
            _logger.LogEventInformation("Processing", record);

            var msg = record.Message.Value;

            var result = await _proxy.Get(msg);
            _logger.LogInformation("Response: {result}", result);
        }

        return record;
    }

    public bool IsValid(ConsumeResult<Ignore, TestEvent> record)
    {
        return true;
    }
}
