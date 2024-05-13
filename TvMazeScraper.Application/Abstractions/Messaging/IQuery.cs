using TvMazeScraper.Domain.Abstractions;
using MediatR;

namespace TvMazeScraper.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}