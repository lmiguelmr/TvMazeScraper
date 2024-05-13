using TvMazeScraper.Domain.Abstractions;
using MediatR;

namespace TvMazeScraper.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}
