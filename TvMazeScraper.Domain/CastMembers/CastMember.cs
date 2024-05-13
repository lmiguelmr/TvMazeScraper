using TvMazeScraper.Domain.Abstractions;

namespace TvMazeScraper.Domain.CastMembers;

public class CastMember : Entity
{
    public CastMember(
        int id,
        string name,
        DateTime? birthday)
        : base(id)
    {
        Name = name;
        Birthday = birthday;
    }

    private CastMember()
    {
    }

    public string Name { get; private set; }

    public DateTime? Birthday { get; internal set; }

    public static CastMember CreateCastMember(
        int id,
        string name,
        DateTime? birthday)
    {
        return new CastMember(id, name, birthday);
    }
}