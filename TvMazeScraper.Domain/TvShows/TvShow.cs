using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.CastMembers;

namespace TvMazeScraper.Domain.TvShows;
public class TvShow : Entity
{
    public TvShow(
        int id,
        string name,
        List<CastMember> castMembers)
        : base(id)
    {
        Name = name;
        CastMembers = castMembers;
    }

    public TvShow()
    {
    }

    public string Name { get; private set; }

    public List<CastMember> CastMembers { get; set; } = [];

    public static TvShow CreateTvShow(
        int id,
        string name,
        List<CastMember> castMembers)
    {
        return new TvShow(id, name, castMembers);
    }
}
