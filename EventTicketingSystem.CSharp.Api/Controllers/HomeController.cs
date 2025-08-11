using EventTicketingSystem.CSharp.Domain.Features.Home;

namespace EventTicketingSystem.CSharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly BL_Home _blHome;

    public HomeController(BL_Home blHome)
    {
        _blHome = blHome;
    }

    [HttpGet()]
    public async Task<IActionResult> Home()
    {
        var data = await _blHome.Home();
        return Ok(data);
    }
}
