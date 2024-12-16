// <copyright file="TmdbService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Text.Json;

    using PanoramaApp.Models;

    public class TmdbService
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private const string BaseUrl = "https://api.themoviedb.org/3/";

        /// <summary>
        /// Initializes a new instance of the <see cref="TmdbService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        public TmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.apiKey = configuration["Tmdb:ApiKey"];
        }

        /// <summary>
        /// Gets the recommendations asynchronous.
        /// </summary>
        /// <param name="tmdbId">The TMDB identifier.</param>
        /// <returns></returns>
        public async Task<List<Movie>> GetRecommendationsAsync(int tmdbId)
        {
            var url = $"{BaseUrl}movie/{tmdbId}/recommendations?api_key={this.apiKey}&language=en-US";
            var response = await this.httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TmdbResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return result?.Results.Select(r => new Movie
                {
                    Title = r.Title,
                }).ToList() ?? new List<Movie>();
            }

            return new List<Movie>();
        }


        /// <summary>
        /// DTO-classes
        /// </summary>
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
