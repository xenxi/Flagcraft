using Flagcraft;
using LaunchDarkly.Sdk.Server;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder
    .Services.AddSingleton((serviceProvider) =>
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var apiKey = configuration["LaunchDarkly:ApiKey"];
        var config = Configuration.Builder(apiKey)
            .StartWaitTime(TimeSpan.FromSeconds(0))
            .Build();
        return new LdClient(config);
    })
    .AddFeatureManagement();
builder.Services.AddSingleton<IFeatureDefinitionProvider, LaunchDarklyFeatureDefinitionProvider>();

var app = builder.Build();
app.RegisterFeatureToggleEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}