namespace TvMazeScraper.Domain.TvShows;

public class ShowEntry
{
    public ShowEntry(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public int Id { get; }

    public string Name { get; }
}
