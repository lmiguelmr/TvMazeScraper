using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net;
using TvMazeScraper.Application.TvShows.AddTvShow;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.TvShows;
using Hangfire;

namespace TvMazeScraper.Infrastructure.TvMazeIntegration;
public class TvMazeJobs : ITvMazeJobs
{
    private readonly int PAGE_LIMIT = 999;

    private readonly ITvMazeApiClient _tvMazeApiClient;
    private readonly ILogger<TvMazeJobs> _logger;
    private readonly ISender _sender;

    public TvMazeJobs(
        ITvMazeApiClient tvMazeApiClient,
        ILogger<TvMazeJobs> logger,
        ISender sender)
    {
        _tvMazeApiClient = tvMazeApiClient;
        _logger = logger;
        _sender = sender;
    }

    public async Task ImportAll(IJobCancellationToken cancellationToken)
    {
        for (int page = 0; page <= PAGE_LIMIT; page++)
        {
            var result = await ProcessPage(page, cancellationToken.ShutdownToken);

            if (result.Error == TvShowErrors.TooManyRequests)
            {
                _logger.LogWarning("Received HTTP 429 error. Retrying in a few seconds.");
                page--;
            }

            if (result.Error == TvShowErrors.NoResults || result.Error == TvShowErrors.ApiRequestFailed)
            {
                _logger.LogError($"{result.Error.Name} ({result.Error.Code}). Stopping ingestion.");
                break;
            }
        }
    }

    public async Task Update(IJobCancellationToken cancellationToken)
    {
        // TODO: Implement logic to check for updates per item and update the database accordingly
    }

    private async Task<Result> ProcessPage(int page, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing page {PageNumber}", page);

            var showsBatch = await _tvMazeApiClient.GetTvShowsAsync(page, cancellationToken).ConfigureAwait(false);

            if (!showsBatch.Any())
            {
                return Result.Failure(TvShowErrors.NoResults);
            }

            foreach (ShowEntry showEntry in showsBatch)
            {
                var result = await ProcessShow(showEntry, cancellationToken);

                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Result.Success();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error processing page {PageNumber}", page);
            return Result.Failure(TvShowErrors.ApiRequestFailed);
        }
    }

    private async Task<Result> ProcessShow(ShowEntry showEntry, CancellationToken cancellationToken = default)
    {
        try
        {
            var cast = await _tvMazeApiClient.GetCastAsync(showEntry.Id, cancellationToken).ConfigureAwait(false);

            if (!cast.Any())
            {
                _logger.LogInformation("No cast members found for show {ShowId}", showEntry.Id);
            }

            var castMemberDtos = cast.Select(castEntry =>
                new CastMemberDto(
                    castEntry.Person.Id,
                    castEntry.Person.Name,
                    DateTime.ParseExact(!string.IsNullOrWhiteSpace(castEntry.Person.Birthday) ? castEntry.Person.Birthday : "1980-02-02", "yyyy-MM-dd", CultureInfo.InvariantCulture)))
            .Distinct()
            .ToList();

            var command = new AddTvShowCommand(showEntry.Id, showEntry.Name, castMemberDtos);

            await _sender.Send(command, cancellationToken);

            _logger.LogInformation("Show {ShowId} processed successfully", showEntry.Id);

            return Result.Success();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogError(ex, "Too many requests received for show {ShowId}", showEntry.Id);
            return Result.Failure(TvShowErrors.TooManyRequests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing show {ShowId}", showEntry.Id);
            return Result.Failure(TvShowErrors.UnknownError);
        }
    }
}
