namespace JWTAuth.WebApi.Endpoints;

public static class WeatherEndpoints
{
    private const string BaseApiUrl = @"api/weather";

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(BaseApiUrl, GetWeatherForecasts);
        return app;
    }

    private static async Task<IResult> GetWeatherForecasts() => Results.Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
    (
        DateTime.Now.AddDays(index),
        Random.Shared.Next(-20, 55),
        Summaries[Random.Shared.Next(Summaries.Length)]
    )).ToArray());

    private record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
