namespace TvMazeScraper.Application.Person.GetAllTvShows;

public class PersonResponse
{
    public PersonResponse(
        int id,
        string name,
        DateTime? birthday)
    {
        Id = id;
        Name = name;
        Birthday = birthday;
    }

    private PersonResponse()
    {
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public DateTime? Birthday { get; init; }

    public static PersonResponse CreatePersonResponse(
        int id,
        string name,
        DateTime? birthday)
    {
        return new PersonResponse(id, name, birthday);
    }
}
