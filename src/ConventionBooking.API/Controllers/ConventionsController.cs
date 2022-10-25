using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Contract;

namespace ConventionBookingApi.Controllers;

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
    /// Signup up for a convention
    /// </summary>
    /// <returns>A ConventionSignup confirmation.</returns>
    [HttpPost]
    [Route("signup")]
    [Authorize("Participant")]
    public IEnumerable<ConventionSignup> Signup(Convention convention)
    {
        // TODO: Implement business logic like
        //       Check that it is possible for the participant to signup for the convention
        //       aka. is the venue limit maxed out?

        // TODO: Find participant given idToken
        var participant = new Participant{ ID = Guid.NewGuid() };

        return Enumerable.Range(1, 1).Select(index => new ConventionSignup
        {
            Participant = participant,
            Convention = convention,
        })
        .ToArray();
    }

    /// <summary>
    /// List all conventions.
    /// </summary>
    /// <returns>A list of Convention</returns>
    [HttpGet]
    [Authorize("Participant")]
    public IEnumerable<Convention> Get()
    {
        return Enumerable.Range(1,3).Select(index => new Convention{
            ID = Guid.NewGuid(),
            Name = "Convention " + index,
            Venue = new Venue { ID = Guid.NewGuid(), Name = "Venue" }
        })
        .ToArray();
    }
}