using System.Data;
using TvMazeScraper.Application.Abstractions.Data;
using TvMazeScraper.Application.Abstractions.Messaging;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.CastMembers;
using TvMazeScraper.Domain.TvShows;

namespace TvMazeScraper.Application.TvShows.AddTvShow;

internal sealed class AddTvShowCommandHandler : ICommandHandler<AddTvShowCommand>
{
    private readonly ISqlConnectionFactory _dbConnection;
    private readonly ITvShowRepository _tvShowRepository;
    private readonly ICastMemberRepository _castMemberRepository;

    public AddTvShowCommandHandler(
        ISqlConnectionFactory dbConnection,
        ITvShowRepository tvShowRepository,
        ICastMemberRepository castMemberRepository)
    {
        _dbConnection = dbConnection;
        _tvShowRepository = tvShowRepository;
        _castMemberRepository = castMemberRepository;
    }

    public async Task<Result> Handle(AddTvShowCommand command, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _dbConnection.CreateConnection();
        using IDbTransaction transaction = connection.BeginTransaction();

        try
        {
            var castMembers = command.CastMembers
                                     .Select(c => CastMember.CreateCastMember(c.Id, c.Name, c.Birthday))
                                     .ToList();
            var castMembersIds = castMembers.Select(cm => cm.Id).ToList();

            // Use separate scopes for each repository operation to avoid concurrent access issues
            var existingCastMembers = await _castMemberRepository.GetAllByIdAsync(castMembersIds, cancellationToken);
            var tvShow = await _tvShowRepository.GetById(command.Id, cancellationToken);

            var newCastMembers = castMembers.Where(c => !existingCastMembers.Any(x => c.Id == x.Id)).ToList();

            if (tvShow == null)
            {
                tvShow = TvShow.CreateTvShow(command.Id, command.Name, newCastMembers);

                await _tvShowRepository.AddAsync(tvShow, cancellationToken);
                await _tvShowRepository.AddRelation(tvShow.Id, castMembersIds, cancellationToken);
            }
            else
            {
                if (newCastMembers.Count != 0)
                {
                    await _castMemberRepository.AddAsync(newCastMembers, cancellationToken);
                }

                await _tvShowRepository.AddMissingCastMembersRelation(tvShow.Id, castMembersIds, cancellationToken);
            }

            transaction.Commit();
            return Result.Success();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Result.Failure<Guid>(TvShowErrors.UnknownError);
        }
    }
}
