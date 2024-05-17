using TvMazeScraper.Application.Shared;

namespace TvMazeScraper.Application.Person.GetAllTvShows;

public class PersonResponse 
{
    public int Id { get; init; }

    public string Name { get; init; }

    public DateTime? Birthday { get; init; }
}
