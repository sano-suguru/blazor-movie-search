using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorMovieSearchApp.Data
{
    public class MovieRepository
    {
        const string baseUrl = "https://www.omdbapi.com";

        readonly IHttpClientFactory _factory;

        public MovieRepository(IHttpClientFactory factory)
            => _factory = factory;

        public async ValueTask<Movie[]> ListAsync(string searchValue)
        {
            HttpClient client = _factory.CreateClient();
            string json = await client.GetStringAsync(
                $"{baseUrl}?s={searchValue}&apikey=4a3b711b");
            Rootobject root = JsonSerializer.Deserialize<Rootobject>(json);

            if (root.Response == "False")
                throw new Exception(root.Error);

            return root.Search;
        }

        class Rootobject
        {
            public Movie[] Search { get; set; }
            public string Response { get; set; }
            public string Error { get; set; }
        }
    }
}
