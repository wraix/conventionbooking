using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Contract;
using ConventionBooking.Core.Repositories;

namespace ConventionBooking.Http.Controllers;

[ApiController]
[Authorize]
[Route("events")]
public class EventsController : ControllerBase
{
    private readonly ILogger<ConventionsController> _logger;

    public EventsController(ILogger<ConventionsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// List all events. Defined as an event that has a convention, venue and a talk. This includes cancelled events.
    /// </summary>
    /// <returns>A list of Event</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IEnumerable<Event> Get()
    {
        // TODO: do connection pooling so each request does not require the API
        //       to make a new connection.
        MySqlRepository r = new MySqlRepository();
        var dbEvents = r.GetAllConventionEvents();

        List<Event> events = new List<Event>();
        foreach ( var ce in dbEvents) {
            events.Add(new Event{
                ID = ce.ID,
                Convention = new Convention { ID = ce.ConventionID, Name = ce.ConventionName },
                Talk = new Talk{ ID = ce.TalkID, Title = ce.TalkTitle, Description = ce.TalkDescription },
                Venue = new Venue {ID = ce.VenueID, Name = ce.VenueName, Address = ce.VenueAddress},
                StartsAt = ce.StartsAt,
                EndsAt = ce.EndsAt,
                NumberOfSeats = ce.NumberOfSeats
            });
        }
        return events.ToArray();
    }
}