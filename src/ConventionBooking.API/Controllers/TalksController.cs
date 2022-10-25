using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Contract;

namespace ConventionBookingApi.Controllers;

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
        // TODO: Find participant given idToken
        var participant = new Participant{ ID = Guid.NewGuid() };

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
    [Authorize("Participant")]
    public IEnumerable<Talk> Get()
    {
        // TODO: Find participant given idToken, decorate result set with signup data.
        var participant = new Participant{ ID = Guid.NewGuid() };

        return Enumerable.Range(1, 5).Select(index => new Talk{
            ID = Guid.NewGuid(),
            Talker = new Talker{ID = Guid.NewGuid(), Name = "John Doe"},
            Title = "Talking about Beer"
        })
        .ToArray();
    }
}