using TvMazeScraper.Application.Abstractions.Messaging;

namespace TvMazeScraper.Application.TvShows.GetAllTvShows;

public sealed record GetAllTvShowsQuery(
    int Page,
    int PageSize) : IQuery<TvShowsResponse>;
