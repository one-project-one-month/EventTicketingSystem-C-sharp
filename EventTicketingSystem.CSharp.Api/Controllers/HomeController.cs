namespace EventTicketingSystem.CSharp.Api.Controllers;

[Tags("Home")]
[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly BL_Home _blHome;

    public HomeController(BL_Home blHome)
    {
        _blHome = blHome;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        var data = await _blHome.GetHome();
        return Ok(data);
    }
}
