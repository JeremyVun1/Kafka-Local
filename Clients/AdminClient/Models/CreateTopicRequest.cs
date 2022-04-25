namespace Api.AdminClient.Models;

public class CreateTopicRequest
{
    public string Name { get; set; }
    public int Partitions { get; set; } = 1;
    public short Replication { get; set; } = 1;
}