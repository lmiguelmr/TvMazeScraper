using TvMazeScraper.Domain.CastMembers;

namespace TvMazeScraper.Infrastructure.TvMazeIntegration.Entries;

public class CastEntry
{
    public Person Person { get; }


    public CastEntry(Person person)
    {
        Person = person;
    }
}

public sealed record Person(int Id, string Name, string Birthday);