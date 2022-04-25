using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Avro;
using Consumer.Processors;
using Consumer.Configuration;
using Consumer.Proxies;
using Models.Kafka.Test;

namespace Consumer.WebJobs;

public class ExampleJob : IJobHost
{
    private readonly ILogger<ExampleJob> _logger;
    private readonly ExampleConfig _config;
    private readonly IConsumer<Ignore, TestEvent> _consumer;
    private readonly IProcessor<Ignore, TestEvent> _processor;

    public ExampleJob(
        ILogger<ExampleJob> logger,
        IOptions<ExampleConfig> config,
        IConsumer<Ignore, TestEvent> consumer,
        IProcessor<Ignore, TestEvent> processor
    )
    {
        _logger = logger;
        _config = config.Value;
        _consumer = consumer;
        _processor = processor;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        string correlationId = null;
        ConsumeResult<Ignore, TestEvent> msg = null;

        try {
            _consumer.Subscribe(_config.Topic);

            while (!ct.IsCancellationRequested)
            {
                try {
                    _logger.LogInformation($"Polling {_config.Topic}...");
                    msg = _consumer.Consume(10000);
                    if (msg == null)
                        continue;

                    correlationId = msg.Message.Value.KafkaHeader.CorrelationId;

                    await _processor.Process(msg);

                    _consumer.StoreOffset(msg);
                }
                catch (ConsumeException ce)
                {
                    // Skip poison pills
                    if (ce.InnerException is AvroException)
                    {
                        var po = ce.ConsumerRecord.TopicPartitionOffset;
                        
                        _logger.LogInformation("Skipping Poison Pill error={e}, partition={p}, offset={o}",
                            ce.Message, po.Partition.Value, po.Offset.Value
                        );
                        _consumer.StoreOffset(po);
                    }
                }
                catch (RetriableException)
                {
                    var po = msg.TopicPartitionOffset;
                    _logger.LogError("Retries Exhausted correlationId={c}, partition={p}, offset={o}",
                        correlationId, po.Partition.Value, po.Offset.Value
                    );
                    _consumer.Assign(po);
                }
                catch(Exception ex)
                {
                    _logger.LogError("Error={e}, correlationId={c}", ex.Message, correlationId);
                }
            }
        }
        catch (OperationCanceledException) { }
    }

    public async Task StopAsync()
    {
        _consumer.Close();
        _logger.LogInformation("Consumer closed");
    }

    // CallAsync not used as we are injecting our own custom JobHost
    public Task CallAsync(string name, IDictionary<string, object> arguments = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
