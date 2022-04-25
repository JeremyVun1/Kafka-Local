using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Models.Kafka.Test;
using Newtonsoft.Json;

namespace Consumer.Extensions;

public static class LoggingExtensions
{
    public static void LogEventInformation(
        this ILogger logger,
        string state,
        ConsumeResult<Ignore, TestEvent> record
    )
    {
        var msg = record.Message.Value;

        logger.LogInformation("{state} event={m}, correlationId={c}, partition={p}, offset={o}",
            state,
            JsonConvert.SerializeObject(new {
                Property1 = msg.Property1,
                Property2 = msg.Property2
            }),
            msg.KafkaHeader.CorrelationId,
            record.Partition.Value,
            record.Offset.Value
        );
    }
}