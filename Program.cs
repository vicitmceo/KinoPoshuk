var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

const string apiKey = "ccdefeff";

app.MapGet("/api/search", async (string title, IHttpClientFactory httpClientFactory) =>
{
    if (string.IsNullOrWhiteSpace(title))
        return Results.BadRequest(new { Error = "Не вказано назву фільму" });

    var client = httpClientFactory.CreateClient();
    var url = $"http://www.omdbapi.com/?apikey={apiKey}&t={Uri.EscapeDataString(title)}";

    var response = await client.GetAsync(url);
    var json = await response.Content.ReadAsStringAsync();

    return Results.Content(json, "application/json");
});

app.Run();
