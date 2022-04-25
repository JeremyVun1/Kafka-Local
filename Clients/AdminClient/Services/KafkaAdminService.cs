using Api.AdminClient.Models;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;

namespace Api.AdminClient;

public interface IKafkaAdminService
{
    Task<bool> CreateTopic(CreateTopicRequest request);
    Task<bool> DeleteTopic(string topicName);
    Task<List<DescribeConfigsResult>> GetTopic(string topicName);
}

public class KafkaAdminService : IKafkaAdminService
{
    private readonly IAdminClient _adminClient;

    public KafkaAdminService(IAdminClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<List<DescribeConfigsResult>> GetTopic(string topicName)
    {
        var resource = new ConfigResource
        {
            Type = ResourceType.Topic,
            Name = topicName
        };

        return await _adminClient.DescribeConfigsAsync(new List<ConfigResource> { resource });
    }

    public async Task<bool> CreateTopic(CreateTopicRequest request)
    {
        var topic = new TopicSpecification
        {
            Name = request.Name,
            NumPartitions = request.Partitions,
            ReplicationFactor = request.Replication
        };

        try
        {
            await _adminClient.CreateTopicsAsync(new List<TopicSpecification> { topic });
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> DeleteTopic(string topicName)
    {
        try
        {
            await _adminClient.DeleteTopicsAsync(new List<string> { topicName });
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

    }
}