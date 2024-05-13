using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TvMazeScraper.Domain.TvShows;
using TvMazeScraper.Application.TvShows.GetAllTvShows;

namespace TvMazeScraper.Api.Controllers.V1;

[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/tvshows")]
public class TvShowsController : ApiControllerBase
{
    private readonly ISender _sender;

    /// <summary>
    ///     <see cref="TvShowsController" /> ctor
    /// </summary>

    public TvShowsController(ISender sender)
    {
        _sender = sender;  
    }

    /// <summary>
    ///     Retrieves a list of TvShows
    /// </summary>
    [HttpGet("GetAll")]
    [SwaggerOperation(Summary = "Get all TvShows", OperationId = "GetAllTvShows")]
    [SwaggerResponse(StatusCodes.Status200OK, "The TvShows retrieval was successful",
        typeof(IEnumerable<TvShow>))]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 200)
    {
        var query = new GetAllTvShowsQuery(page, pageSize);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
