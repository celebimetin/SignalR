using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.API.Hubs;

namespace SignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<MyHub> _hubContext;

        public NotificationsController(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("{teamCount}")]
        public async Task<IActionResult> SetTeamConut(int teamCount)
        {
            MyHub.TeamCount = teamCount;
            await _hubContext.Clients.All.SendAsync("Notify", $"Arkadaşlar takım {teamCount} kişi olacak");
            return Ok();
        }
    }
}