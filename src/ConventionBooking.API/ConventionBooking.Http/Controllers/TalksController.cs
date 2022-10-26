using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Contract;
using ConventionBooking.Core.Repositories;

namespace ConventionBooking.Http.Controllers;

[ApiController]
[Authorize]
[Route("talks")]
public class TalksController : ControllerBase
{
    private readonly ILogger<ConventionsController> _logger;

    public TalksController(ILogger<ConventionsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// List all talks. A dimension table of talks, including unscheduled talks and only just created talks.
    /// </summary>
    /// <returns>A list of talks</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IEnumerable<Talk> Get()
    {
        // TODO: do connection pooling so each request does not require the API
        //       to make a new connection.
        MySqlRepository r = new MySqlRepository();
        var dbTalks = r.GetAllTalks();

        List<Talk> _talks = new List<Talk>();
        foreach ( var ce in dbTalks) {
            _talks.Add(new Talk{
                ID = ce.ID,
                Title = ce.Title,
                Description = ce.Description
            });
        }
        return _talks.ToArray();
    }
}