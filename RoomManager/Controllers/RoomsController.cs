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
        if (activeOnly == true) rooms = rooms.Where(r => r.IsActive);
        
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

    [HttpPost]
    public ActionResult<Room> CrateRoom(Room room)
    {
        room.Id = InMemoryDB.Rooms.Any()
            ? InMemoryDB.Rooms.Max(r => r.Id) + 1
            : 1;
        
        InMemoryDB.Rooms.Add(room);
        
        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public ActionResult<Room> PutRoom(int id, Room updatedRoom)
    {
        var room = InMemoryDB.Rooms.FirstOrDefault(r => r.Id == id);
        
        if (room is null) return NotFound();
        
        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = InMemoryDB.Rooms.FirstOrDefault(r  => r.Id == id);
        if (room is null) return NotFound();
        bool hasReservation = InMemoryDB.Reservations.Any(r => r.RoomId == id);
        if (hasReservation) return Conflict("Cannot delete room because it has related reservations.");
        InMemoryDB.Rooms.Remove(room);
        return NoContent();
    }
}