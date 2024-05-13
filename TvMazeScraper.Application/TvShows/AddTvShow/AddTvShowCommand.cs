using TvMazeScraper.Application.Abstractions.Messaging;

namespace TvMazeScraper.Application.TvShows.AddTvShow;

public sealed record AddTvShowCommand(
    int Id, 
    string Name,
    List<CastMemberDto> CastMembers) : ICommand;
