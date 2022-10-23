using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ConventionBooking.Contract;

namespace ConventionBookingApi.Controllers;

[ApiController]
[Route("conventions")]
public class ConventionsController : ControllerBase
{
    private readonly ILogger<ConventionsController> _logger;

    public ConventionsController(ILogger<ConventionsController> logger)
    {
        _logger = logger;
    }

    // POST: conventions/signup
    [HttpPost]
    [Route("signup")]
    [Authorize("Participant")]
    public IEnumerable<ConventionSignup> Signup(Convention convention)
    {
        return Enumerable.Range(1, 1).Select(index => new ConventionSignup
        {
            Name = "",
        })
        .ToArray();
    }

    // GET: conventions/
    [HttpGet]
    public IEnumerable<Convention> Get()
    {
        return Enumerable.Range(1,3).Select(index => new Convention{
            ID = Guid.NewGuid(),
        })
        .ToArray();
    }
}