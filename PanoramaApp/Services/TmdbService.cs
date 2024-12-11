using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PanoramaApp.Models;

namespace PanoramaApp.Services
{
public class TmdbService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.themoviedb.org/3/";

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Tmdb:ApiKey"]; // Hämta API-nyckeln från appsettings.json
    }

public async Task<List<Movie>> GetRecommendationsAsync(int tmdbId)
{
    var url = $"{BaseUrl}movie/{tmdbId}/recommendations?api_key={_apiKey}&language=en-US";
    var response = await _httpClient.GetAsync(url);

    if (response.IsSuccessStatusCode)
    {
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TmdbResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

return result?.Results.Select(r => new Movie
{
    Title = r.Title
}).ToList() ?? new List<Movie>();

    }

    // Om API-anropet misslyckas, returnera en tom lista
    return new List<Movie>();
}


// DTO-klasser för att hantera TMDb:s JSON-respons
public class TmdbResponse
{
    public List<TmdbMovieResponse> Results { get; set; }
}

public class TmdbMovieResponse
{
    public string Title { get; set; }
}
}
}