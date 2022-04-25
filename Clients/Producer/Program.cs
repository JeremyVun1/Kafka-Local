using Models.Kafka.Test;
using Producer.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.secret.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services
    .BindConfigurations(config)
    .AddSchemaRegistry(config)
    .AddProducer<TestEvent>(config)
    .AddServices();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
