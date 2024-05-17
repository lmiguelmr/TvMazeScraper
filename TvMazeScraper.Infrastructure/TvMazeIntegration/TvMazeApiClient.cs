using Newtonsoft.Json;
using TvMazeScraper.Domain.TvShows;
using TvMazeScraper.Infrastructure.TvMazeIntegration.Entries;

namespace TvMazeScraper.Infrastructure.TvMazeIntegration;

public class TvMazeApiClient : ITvMazeApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TvMazeApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<ShowEntry>> GetTvShowsAsync(int page, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<IEnumerable<ShowEntry>>($"/shows?page={page}", cancellationToken);
    }

    public async Task<IEnumerable<CastEntry>> GetCastAsync(int showId, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<IEnumerable<CastEntry>>($"/shows/{showId}/cast", cancellationToken);
    }


    private async Task<T> SendRequestAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("TvMazeApiClient");
            var response = await client.GetAsync(requestUri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"The HTTP request failed with status code {(int)response.StatusCode} ({response.ReasonPhrase}).";
                throw new HttpRequestException(errorMessage);
            }

            string content = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonConvert.DeserializeObject<T>(content);
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("An error occurred while making the API request.", ex);
        }
    }
}
