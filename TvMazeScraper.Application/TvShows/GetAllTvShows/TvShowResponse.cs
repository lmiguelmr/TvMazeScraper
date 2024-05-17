using TvMazeScraper.Application.Person.GetAllTvShows;
using TvMazeScraper.Application.Shared;

namespace TvMazeScraper.Application.TvShows.GetAllTvShows;

public class TvShowResponse
{
    public int Id { get; init; }

    public string Name { get; init; }

    public List<PersonResponse> Cast { get; init; } = [];
}
