using TvMazeScraper.Application.Abstractions.Messaging;
using TvMazeScraper.Application.Person.GetAllTvShows;
using TvMazeScraper.Application.Shared;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.TvShows;

namespace TvMazeScraper.Application.TvShows.GetAllTvShows;

internal sealed class GetAllTvShowsQueryHandler
    : IQueryHandler<GetAllTvShowsQuery, PagedResponse<TvShowResponse>>
{
    private readonly ITvShowRepository _tvShowRepository;

    public GetAllTvShowsQueryHandler(ITvShowRepository tvShowRepository)
    {
        _tvShowRepository = tvShowRepository;
    }

    public async Task<Result<PagedResponse<TvShowResponse>>> Handle(GetAllTvShowsQuery request, CancellationToken cancellationToken)
    {
        var (tvShows, totalCount) = await _tvShowRepository.GetAllAsync2(request.Page, request.PageSize, cancellationToken);

        if (tvShows == null || !tvShows.Any())
        {
            return new PagedResponse<TvShowResponse>();
        }

        var tvShowResponse = new PagedResponse<TvShowResponse>
        {
            Items = tvShows.Select(t => new TvShowResponse
            {
                Id = t.Id,
                Name = t.Name,
                Cast = t.CastMembers.Select(c => new PersonResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Birthday = c.Birthday,
                }).ToList()
            }).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };

        return tvShowResponse;
    }
}
