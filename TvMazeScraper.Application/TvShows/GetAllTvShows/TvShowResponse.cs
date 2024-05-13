using TvMazeScraper.Application.Person.GetAllTvShows;

namespace TvMazeScraper.Application.TvShows.GetAllTvShows;

public class TvShowResponse
{
    public TvShowResponse(
        int id,
        string name,
        List<PersonResponse> cast)
    {
        Id = id;
        Name = name;
        Cast = cast;
    }

    private TvShowResponse()
    {
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public List<PersonResponse> Cast { get; init; } = [];

    public static TvShowResponse CreateTvShowResponse(
        int id, 
        string name, 
        List<PersonResponse> cast)
    {
        return new TvShowResponse(id, name, cast);
    }
}
