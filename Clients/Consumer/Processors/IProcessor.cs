using Confluent.Kafka;

namespace Consumer.Processors;

public interface IProcessor<TKey, TValue>
{
    Task<object> Process(ConsumeResult<TKey, TValue> record);
}