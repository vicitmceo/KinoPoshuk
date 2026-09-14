using System.Text.Json;
using KinoPoshuk.DAL.Constants;
using KinoPoshuk.DAL.DTO;
using KinoPoshuk.DAL.Exceptions;
using KinoPoshuk.DAL.Interfaces;
using Microsoft.Extensions.Configuration;

namespace KinoPoshuk.DAL.ExternalServices;

public class OmdbApiClient : IOmdbApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OmdbApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Omdb:ApiKey"] ?? throw new InvalidOperationException("Omdb:ApiKey не налаштовано");
    }

    public async Task<MovieDto?> GetByTitleAsync(string title)
    {
        var url = $"{OmdbConstants.BaseUrl}?apikey={_apiKey}&t={Uri.EscapeDataString(title)}";

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
        }
        catch (HttpRequestException ex)
        {
            throw new ExternalApiException($"OMDb API недоступний: {ex.Message}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var raw = JsonSerializer.Deserialize<OmdbRawResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (raw is null || raw.Response == "False") return null;

        return new MovieDto
        {
            ImdbId = raw.ImdbId,
            Title = raw.Title,
            Year = int.TryParse(raw.Year[..Math.Min(4, raw.Year.Length)], out var year) ? year : 0,
            Genre = raw.Genre,
            Director = raw.Director,
            Actors = raw.Actors,
            Plot = raw.Plot,
            PosterUrl = raw.Poster,
            ImdbRating = raw.ImdbRating,
            Runtime = raw.Runtime
        };
    }
}
