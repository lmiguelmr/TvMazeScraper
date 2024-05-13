using TvMazeScraper.Application.Abstractions.Messaging;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.CastMembers;
using TvMazeScraper.Domain.JointTables;
using TvMazeScraper.Domain.TvShows;

namespace TvMazeScraper.Application.TvShows.AddTvShow;

internal sealed class AddTvShowCommandHandler : ICommandHandler<AddTvShowCommand>
{
    private readonly ITvShowRepository _tvShowRepository;
    private readonly ICastMemberRepository _castMemberRepository;
    private readonly ITvShowCastMemberRepository _tvShowCastMemberRepository;
    private readonly IUnitOfWork _uowManager;

    public AddTvShowCommandHandler(
        ITvShowRepository tvShowRepository,
        ICastMemberRepository castMemberRepository,
        ITvShowCastMemberRepository tvShowCastMemberRepository,
        IUnitOfWork uowManager)
    {
        _tvShowRepository = tvShowRepository;
        _castMemberRepository = castMemberRepository;
        _tvShowCastMemberRepository = tvShowCastMemberRepository;
        _uowManager = uowManager;
    }
    public async Task<Result> Handle(AddTvShowCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _uowManager.StartUnitOfWork();

            var tvShow = await _tvShowRepository.Add(TvShow.CreateTvShow(command.Id, command.Name));

            var castMembersIds = command.CastMembers.Select(c => new CastMember(c.Id, c.Name, c.Birthday));
            await _castMemberRepository.AddNewCastMembersAsync(castMembersIds, cancellationToken);

            var tvShowCastMembers = command.CastMembers.Select(c => new TvShowCastMember { TvShowId = tvShow.Id, CastMemberId = c.Id });

            var tvShowCastMembersToAdd = new List<TvShowCastMember>();
            foreach (var tvShowCastMember in tvShowCastMembers)
            {
                var existingRelation = await _tvShowCastMemberRepository.ExistsAsync(tvShowCastMember.TvShowId, tvShowCastMember.CastMemberId, cancellationToken);

                if (!existingRelation)
                    tvShowCastMembersToAdd.Add(tvShowCastMember);
            }

            await _tvShowCastMemberRepository.AddRange(tvShowCastMembersToAdd, cancellationToken);

            await _uowManager.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(TvShowErrors.UnknownError);
        }
    }
}
