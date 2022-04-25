using System.Net;
using Consumer.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Consumer.Configuration;

public static class HttpClientsConfiguration
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient<IExampleProxy, ExampleProxy>()
            .AddPolicyHandler(GetRetryPolicy(config));

        return services;
    }

    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IConfiguration config)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                int.Parse(config["Api:MaxRetries"]),
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
        
        var fallbackPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .FallbackAsync(
                new HttpResponseMessage(HttpStatusCode.InternalServerError),
                (result, context) => throw new RetriableException()
            );

        return fallbackPolicy.WrapAsync(retryPolicy);
    }
}