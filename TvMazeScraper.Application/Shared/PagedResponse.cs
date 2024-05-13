namespace TvMazeScraper.Application.Shared;

public class PagedResponse<T>
{
    public List<T> Items { get; init; } = [];

    public int PageSize { get; init; }

    public int Page { get; init; }

    public int TotalCount { get; init; }

    public bool HasNextPage => TotalCount > (Page * PageSize);

    public bool HasPreviousPage => Page > 1;
}
