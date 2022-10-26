using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ConventionBooking.Contract;
using ConventionBooking.Core.Repositories;

namespace ConventionBooking.Http.Controllers;

[ApiController]
[Authorize]
[Route("registrations")]
public class RegistrationsController : ControllerBase
{
    private readonly ILogger<RegistrationsController> _logger;

    public RegistrationsController(ILogger<RegistrationsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Signup up for a talk
    /// </summary>
    /// <returns>A SeatReservation confirmation.</returns>
    [HttpPost]
    [Route("seats")]
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
    /// Signup up for a convention
    /// </summary>
    /// <returns>A ConventionSignup confirmation.</returns>
    [HttpPost]
    [Route("conventions")]
    [Authorize("Participant")]
    public IEnumerable<ConventionSignup> Signup(Convention convention)
    {
        // TODO: Implement business logic like
        //       Check that it is possible for the participant to signup for the convention
        //       aka. is the venue limit maxed out?

        // TODO: Find participant given idToken
        var participant = new Participant{ ID = Guid.NewGuid() };

        // TODO: do connection pooling so each request does not require the API
        //       to make a new connection.
        MySqlRepository r = new MySqlRepository();
        var dbTalks = r.GetAllTalks();

        return Enumerable.Range(1, 1).Select(index => new ConventionSignup
        {
            Participant = participant,
            Convention = convention,
        })
        .ToArray();
    }

    /// <summary>
    /// Lists a participants registrations be it reserved seats or convention signups.
    /// </summary>
    /// <returns>A list of registrations</returns>
    [HttpGet]
    [Authorize("Participant")]
    public Registrations Get()
    {
        // TODO: Find participant given idToken, decorate result set with signup data.
        var participant = new Participant{ ID = Guid.NewGuid() };

        // Lookup SeatReservation for Participant
        // Lookup ConventionSignup for Participant

        var registrations = new Registrations {
            ReservedSeatsAtTalks = new List<SeatReservation>(),
            SignedUpForConventions = new List<ConventionSignup>()
        };

        return registrations;
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