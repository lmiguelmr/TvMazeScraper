using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace TvMazeScraper.Api.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ApiControllerBase : ControllerBase
{
}
