using TvMazeScraper.Application.Abstractions.Messaging;
using TvMazeScraper.Application.Person.GetAllTvShows;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.CastMembers;
using TvMazeScraper.Domain.JointTables;
using TvMazeScraper.Domain.TvShows;

namespace TvMazeScraper.Application.TvShows.GetAllTvShows;

internal sealed class GetAllTvShowsQueryHandler : IQueryHandler<GetAllTvShowsQuery, TvShowsResponse>
{
    private readonly ICastMemberRepository _castMemberRepository;
    private readonly ITvShowRepository _tvShowRepository;
    private readonly ITvShowCastMemberRepository _tvShowCastMemberRepository;

    public GetAllTvShowsQueryHandler(
        ICastMemberRepository castMemberRepository,
        ITvShowRepository tvShowRepository,
        ITvShowCastMemberRepository tvShowCastMemberRepository)
    {
        _castMemberRepository = castMemberRepository;
        _tvShowRepository = tvShowRepository;
        _tvShowCastMemberRepository = tvShowCastMemberRepository;
    }

    public async Task<Result<TvShowsResponse>> Handle(GetAllTvShowsQuery request, CancellationToken cancellationToken)
    {
        var tvShows = await _tvShowRepository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        if(tvShows == null || !tvShows.Any())
        {
            return Result.Failure<TvShowsResponse>(TvShowErrors.NoResults);
        }
        var totalCount = await _tvShowRepository.GetTvShowCountAsync(cancellationToken);

        var tvShowsResponse = new TvShowsResponse
        {
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };
        
        foreach (var tvShow in tvShows)
        {
            var tvShowResponse = TvShowResponse.CreateTvShowResponse(tvShow.Id, tvShow.Name.ToString(), []);

            var castIds = await _tvShowCastMemberRepository.GetCastIdsForTvShowAsync(tvShow.Id, cancellationToken);

            var cast = await _castMemberRepository.GetAllAsync(castIds, cancellationToken);
            var personResponses = cast.Select(c => PersonResponse.CreatePersonResponse(c.Id, c.Name, c.Birthday));

            tvShowResponse.Cast.AddRange(personResponses);
            tvShowsResponse.Items.Add(tvShowResponse);
        }
        
        return tvShowsResponse;
    }
}
