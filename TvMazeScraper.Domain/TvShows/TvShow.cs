using TvMazeScraper.Domain.Abstractions;

namespace TvMazeScraper.Domain.TvShows;
public sealed class TvShow : Entity
{
    public TvShow(
        int id,
        string name)
        : base(id)
    {
        Name = name;
    }

    private TvShow()
    {
    }

    public string Name { get; private set; }

    public static TvShow CreateTvShow(
        int id,
        string name)
    {
        return new TvShow(id, name);
    }
}
