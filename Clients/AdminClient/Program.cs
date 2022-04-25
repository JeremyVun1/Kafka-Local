using Api.AdminClient.Configuration;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services
    .BindOptions(config)
    .AddKafkaServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
