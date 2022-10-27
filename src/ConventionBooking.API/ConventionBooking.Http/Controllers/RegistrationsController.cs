using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ConventionBooking.Contract;
using ConventionBooking.Core.Models;
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
    public IEnumerable<ConventionBooking.Contract.SeatReservation> Signup(ConventionBooking.Contract.EventReference ev)
    {
        MySqlRepository r = new MySqlRepository();

        var participant = createOrLookupParticipant(GetSubject());

        // TODO: Implement business logic like
        //       Check that it is possible for the participant to reserve the seat
        //       aka. is the number of seats maxed out?

        // Sanity check existance and read for response hydration
        var conventionEvent = r.GetConventionEventViewFromTalkID(ev.ID);
        if (conventionEvent == null) {
            throw new Exception("event not found");
            //return new List<ConventionBooking.Contract.SeatReservation>(); // return the empty set.
        }

        // Marshal Contracts to Models
        PersonReference _participant = new PersonReference(participant.ID);
        ConventionEventReference _event = new ConventionEventReference(conventionEvent.ID);

        var newSeatingReservationRef = r.CreateSeatingReservation(_event, _participant);

        // Marshal Models to Contracts
        var l = new List<ConventionBooking.Contract.SeatReservation>{
            new ConventionBooking.Contract.SeatReservation
            {
                Participant = participant,
                Event = new Event{
                    ID = conventionEvent.ID,
                    Convention = new Contract.Convention
                    {
                        ID = conventionEvent.ConventionID,
                        Name = conventionEvent.ConventionName
                    },
                    Talk = new Contract.Talk
                    {
                        ID = conventionEvent.TalkID,
                        Title = conventionEvent.TalkTitle,
                        Description = conventionEvent.TalkDescription,
                        Talker = new Contract.Talker
                        {
                            ID = conventionEvent.TalkTalkerID
                        }
                    },
                    Venue = new Contract.Venue
                    {
                        ID = conventionEvent.VenueID,
                        Name = conventionEvent.VenueName,
                        Address = conventionEvent.VenueAddress
                    },
                    StartsAt = conventionEvent.StartsAt,
                    EndsAt = conventionEvent.EndsAt,
                    NumberOfSeats = conventionEvent.NumberOfSeats
                }
            }
        };

        _logger.LogInformation($"created seat reservation {newSeatingReservationRef.ID}");

        return l.ToArray();
    }

    /// <summary>
    /// Signup up for a convention
    /// </summary>
    /// <returns>A ConventionSignup confirmation.</returns>
    [HttpPost]
    [Route("conventions")]
    [Authorize("Participant")]
    public IEnumerable<ConventionSignup> Signup(ConventionBooking.Contract.ConventionReference convention)
    {
        // TODO: do connection pooling so each request does not require the API
        //       to make a new connection. This will severely impact scaling of the api.
        MySqlRepository r = new MySqlRepository();

        var participant = createOrLookupParticipant(GetSubject());

        // TODO: Implement business logic like
        //       Check that it is possible for the participant to signup for the convention
        //       aka. is the venue limit maxed out?

        // Sanity check existance and read for response hydration
        var conventionEvent = r.GetAtleastOneConventionEventByConventionID(convention.ID);
        if (conventionEvent == null) {
            throw new Exception("event with convention id not found");
        }

        // Marshal Contracts to Models
        PersonReference _participant = new PersonReference(participant.ID);
        ConventionEventReference _event = new ConventionEventReference(conventionEvent.ConventionID);

        var newConventionRegistration = r.CreateConventionRegistration(_event, _participant);

        // Marshal Models to Contract
        var l = new List<ConventionBooking.Contract.ConventionSignup>{
            new ConventionBooking.Contract.ConventionSignup
            {
                Participant = participant,
                Convention = new Contract.Convention
                {
                    ID = convention.ID,
                    Name = conventionEvent.ConventionName
                }
            }
        };

        _logger.LogInformation($"created convetion registration {newConventionRegistration.ID}");

        return l.ToArray();
    }

    /// <summary>
    /// Lists a participants registrations be it reserved seats or convention signups.
    /// </summary>
    /// <returns>A list of registrations</returns>
    [HttpGet]
    [Authorize("Participant")]
    public Registrations Get()
    {
        // TODO: do connection pooling so each request does not require the API
        //       to make a new connection. This will severely impact scaling of the api.
        MySqlRepository r = new MySqlRepository();

        var participant = createOrLookupParticipant(GetSubject());

        // Marshal Contracts to Models
        PersonReference _participant = new PersonReference(participant.ID);

        var dbRegistrations = r.GetAllRegistrations(_participant);

        List<ConventionSignup> _conventionSignups = new List<ConventionSignup>();
        List<SeatReservation> _seatReservations = new List<SeatReservation>();

        foreach ( var reg in dbRegistrations) {
            if (reg.IsConventionRegistration > 0) {
                _conventionSignups.Add(new ConventionSignup{
                    Convention = new Contract.Convention
                    {
                        ID = reg.ConventionID,
                        Name = reg.ConventionName
                    },
                    Participant = new Participant{ID = participant.ID}
                });
            }

            if (reg.IsSeatReservation > 0) {
                _seatReservations.Add(new SeatReservation{
                    ID = reg.SeatingReservationID,
                    Participant = new Participant{ID = participant.ID},
                    Event = new Event{
                        ID = reg.ID,
                        Convention = new Contract.Convention
                        {
                            ID = reg.ConventionID,
                            Name = reg.ConventionName
                        },
                        Talk = new Contract.Talk
                        {
                            ID = reg.TalkID,
                            Title = reg.TalkTitle,
                            Description = reg.TalkDescription,
                            Talker = new Contract.Talker
                            {
                                ID = reg.TalkTalkerID
                            }
                        },
                        Venue = new Contract.Venue
                        {
                            ID = reg.VenueID,
                            Name = reg.VenueName,
                            Address = reg.VenueAddress
                        },
                        StartsAt = reg.StartsAt,
                        EndsAt = reg.EndsAt,
                        NumberOfSeats = reg.NumberOfSeats
                    }
                });
            }
        }

        var registrations = new Registrations {
            participant = participant,
            ReservedSeatsAtTalks = _seatReservations,
            SignedUpForConventions = _conventionSignups
        };

        return registrations;
    }


    private Claim GetSubject() {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) // Guard
        {
            throw new Exception("ClaimsIdentity not found");
        }

        var sub = identity.FindFirst(ClaimTypes.NameIdentifier);
        if (sub == null) {
            throw new Exception("claim nameidentifier not found");
        }
        return sub;
    }

    private Participant createOrLookupParticipant(Claim sub) {
        MySqlRepository r = new MySqlRepository();
        var person = r.LookupPersonReferenceBySub(sub.Value);
        if (person == null) {
            person = r.CreatePerson(sub.Value);
        }

        return new Participant{ID = person.ID};
    }
}