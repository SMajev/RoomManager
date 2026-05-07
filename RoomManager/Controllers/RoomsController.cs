using Microsoft.AspNetCore.Mvc;
using RoomManager.Models;

namespace RoomManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Room>> GetRooms(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly
    )
    {
        
    }
}