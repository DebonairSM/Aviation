using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace AzureMicroservicesPlatform.Tests;

public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/weatherforecast");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsFiveForecasts()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var forecasts = await client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        // Assert
        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts.Length);
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
