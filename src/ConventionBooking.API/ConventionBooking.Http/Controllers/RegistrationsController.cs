using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Contract;

namespace ConventionBookingApi.Http.Controllers;

[ApiController]
[Authorize]
[Route("registrations")]
public class RegistrationsContontroller : ControllerBase
{
    private readonly ILogger<RegistrationsContontroller> _logger;

    public RegistrationsContontroller(ILogger<RegistrationsContontroller> logger)
    {
        _logger = logger;
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
}