using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Contract;
using ConventionBooking.Core.Repositories;

namespace ConventionBooking.Http.Controllers;

[ApiController]
[Authorize]
[Route("conventions")]
public class ConventionsController : ControllerBase
{
    private readonly ILogger<ConventionsController> _logger;

    public ConventionsController(ILogger<ConventionsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// List all conventions. A dimension table of conventions including unscheduled ones and only just created ones.
    /// </summary>
    /// <returns>A list of Events</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IEnumerable<Convention> Get()
    {
        // TODO: do connection pooling so each request does not require the API
        //       to make a new connection.
        MySqlRepository r = new MySqlRepository();
        var dbConventions = r.GetAllConventions();

        List<Convention> _conventions = new List<Convention>();
        foreach ( var ce in dbConventions) {
            _conventions.Add(new Convention{
                ID = ce.ID,
                Name = ce.Name
            });
        }
        return _conventions.ToArray();
    }
}