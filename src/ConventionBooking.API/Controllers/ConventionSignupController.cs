using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Model;

namespace ConventionBookingApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ConventionSignupController : ControllerBase
{
    private readonly ILogger<ConventionSignupController> _logger;

    public ConventionSignupController(ILogger<ConventionSignupController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "signup")]
    [Authorize("Participant")]
    public IEnumerable<ConventionSignup> Signup(Convention convention)
    {
        return Enumerable.Range(1, 1).Select(index => new ConventionSignup
        {
            Name = "",
        })
        .ToArray();
    }

    [HttpGet(Name = "read")]
    public IEnumerable<Convention> Get()
    {
        return Enumerable.Range(1,3).Select(index => new Convention{
            ID = Guid.NewGuid(),
        })
        .ToArray();
    }
}