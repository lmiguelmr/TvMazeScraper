using TvMazeScraper.Application.Abstractions.Messaging;
using TvMazeScraper.Application.Shared;

namespace TvMazeScraper.Application.TvShows.GetAllTvShows;

public sealed record GetAllTvShowsQuery(
    int Page,
    int PageSize) : IQuery<PagedResponse<TvShowResponse>>;
