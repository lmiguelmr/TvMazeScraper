using TvMazeScraper.Domain.Abstractions;

namespace TvMazeScraper.Domain.TvShows;

public static class TvShowErrors
{
    public static Error NotFound = new(
        "TvShow.NotFound",
        "The TV show with the specified identifier was not found");

    public static Error ExistingTvShow = new(
        "TvShow.NotFound",
        "The TV show with the specified identifier was not found");

    public static Error CastNotFound = new(
        "TvShow.CastNotFound",
        "The cast members for the TV show could not be retrieved");

    public static Error ApiRequestFailed = new(
        "TvShow.ApiRequestFailed",
        "Failed to fetch data from the external API");

    public static Error RequestTimeout = new(
    "TvShow.RequestTimeout",
    "The request to fetch data from the external API timed out");

    public static Error TooManyRequests = new(
        "TvShow.TooManyRequests",
        "The request to fetch data from the external API encountered rate limiting");

    public static Error NoResults = new(
        "TvShow.NoResults",
        "No results found.");

    public static Error UnknownError = new(
    "TvShow.UnknownError",
    "An unknown error occurred while processing the TV show");

}
