using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Contract;
using System.Security.Claims;

namespace ConventionBookingApi.Http.Controllers;

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
    /// Signup up for a talk
    /// </summary>
    /// <returns>A SeatReservation confirmation.</returns>
    [HttpPost]
    [Route("signup")]
    [Authorize("Participant")]
    public IEnumerable<SeatReservation> Signup(Talk talk)
    {
        var participant = lookupParticipant();

        // TODO: Implement business logic like
        //       Check that it is possible for the participant to reserve the seat
        //       aka. is the number of seats maxed out?

        // TODO: Register in storage

        return Enumerable.Range(1, 1).Select(index => new SeatReservation
        {
            Participant = participant,
            Talk = talk,
        })
        .ToArray();
    }

    /// <summary>
    /// List all talks.
    /// </summary>
    /// <returns>A list of talks</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IEnumerable<Talk> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Talk{
            ID = Guid.NewGuid(),
            Talker = new Talker{ID = Guid.NewGuid(), Name = "John Doe"},
            Title = "Talking about Beer"
        })
        .ToArray();
    }

    // Lookup participant from authorized identity.
    private Participant lookupParticipant() {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) // Guard
        {
            throw new Exception("ClaimsIdentity not found");
        }

        var sub = identity.FindFirst(ClaimTypes.NameIdentifier);
        if (sub == null) {
            throw new Exception("claim nameidentifier not found");
        }

        var participants = new List<Participant>{
            new Participant{ ID = Guid.NewGuid(), Sub = "qwerty" },
            new Participant{ ID = Guid.NewGuid(), Sub = sub.Value },
            new Participant{ ID = Guid.NewGuid(), Sub = "12345" }
        };

        Participant? foundParticipant = null;
        foreach (Participant p in participants) {
            if (p.Sub == sub.Value) {
                foundParticipant = p;
                break;
            }
        }

        if (foundParticipant == null) {
            throw new Exception("participant not found");
        }
        return foundParticipant;

        /*IEnumerable<Claim> claims = identity.Claims;
        foreach (Claim c in claims)
        {
            if (c.Type == ClaimTypes.NameIdentifier) {
                sub = c.Value;
            }
            _logger.LogInformation($"Claim: {c}");
        }
        _logger.LogInformation($"Subject: {sub}");*/

        /*if (sub.Length <= 0) // Guard
        {
          throw new Exception("participant not found");
        }*/
    }
}