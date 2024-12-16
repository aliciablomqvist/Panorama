// <copyright file="GenreService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class GenreService
    {
        private readonly HttpClient httpClient;

        public GenreService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<string>> FetchGenresAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://moviesdatabase.p.rapidapi.com/titles/utils/genres"),
            };
            request.Headers.Add("x-rapidapi-key", "67a961b759mshd87ba4ce7aea481p1aab7cjsn78a745e1dd6a");
            request.Headers.Add("x-rapidapi-host", "moviesdatabase.p.rapidapi.com");

            var response = await this.httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var genres = JsonSerializer.Deserialize<GenreResponse>(content);
            return genres?.Results ?? new List<string>();
        }

        public class GenreResponse
        {
            public List<string> Results { get; set; } = new ();
        }
    }
}
