using Microsoft.AspNetCore.Mvc;
using RoomManager.Data;
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
        var rooms = InMemoryDB.Rooms.AsQueryable();
        
        if (minCapacity.HasValue) rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        if (hasProjector.HasValue) rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        if (activeOnly == true) rooms.Where(r => r.IsActive == true);
        
        return Ok(rooms.ToList());
    }


    [HttpGet("{id}")]
    public ActionResult<Room> GetRoom(int id)
    {
        var room = InMemoryDB.Rooms.FirstOrDefault(r => r.Id == id);
        
        if (room is null)
            return NotFound();

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetRoomsByBuilding(string buildingCode)
    {
        var rooms = InMemoryDB.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Ok(rooms);
    }
}