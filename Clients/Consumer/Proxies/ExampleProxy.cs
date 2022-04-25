using System.Text;
using Microsoft.Extensions.Logging;
using Models.Kafka.Test;
using Newtonsoft.Json;

namespace Consumer.Proxies;

public interface IExampleProxy
{
    Task<bool> Get(TestEvent msg);
}

public class ExampleProxy : IExampleProxy
{
    private readonly HttpClient _client;
    private readonly ILogger<ExampleProxy> _logger;

    public ExampleProxy(HttpClient client, ILogger<ExampleProxy> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<bool> Get(TestEvent msg)
    {
        return true;
    }
}