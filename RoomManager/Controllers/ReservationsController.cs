using Microsoft.AspNetCore.Mvc;
using RoomManager.Data;
using RoomManager.Models;

namespace RoomManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Reservation>> GetReservations(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = InMemoryDB.Reservations.AsQueryable();
        if (date.HasValue)
            reservations = reservations.Where(r => r.Date == date.Value);
        if (!string.IsNullOrWhiteSpace(status))
            reservations = reservations.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        if (roomId.HasValue) reservations = reservations.Where(r => r.RoomId == roomId.Value);
        return Ok(reservations);
    }


    [HttpGet("{id}")]
    public ActionResult<Reservation> GetReservation(int id)
    {
        var reservation = InMemoryDB.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null) return NotFound();
        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<List<Reservation>> CreateReservation(Reservation reservation)
    {
        var room = InMemoryDB.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        
        if (room is null) return BadRequest("Room does not exist.");
        
        if (!room.IsActive) return BadRequest("Room is not active.");
        
        if (HasTimeConflict(reservation)) return Conflict("Reservation conflicts with another reservation for this room.");

        reservation.Id = InMemoryDB.Reservations.Any()
            ? InMemoryDB.Reservations.Max(r => r.Id) + 1
            : 1;
        
        InMemoryDB.Reservations.Add(reservation);
        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public ActionResult<List<Reservation>> UpdateReservation(int id, Reservation updatedReservation)
    {
        var reservation = InMemoryDB.Reservations.FirstOrDefault(r => r.Id == id);
        
        if (reservation is null) return NotFound();
        
        var room = InMemoryDB.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
        
        if (room is null) return BadRequest("Room does not exist.");
        if (!room.IsActive) return BadRequest("Room is not active.");
        if (HasTimeConflict(updatedReservation, id)) return Conflict("Reservation conflicts with another reservation for this room.");

        
        reservation.RoomId = updatedReservation.RoomId;
        reservation.OrganizerName = updatedReservation.OrganizerName;
        reservation.Topic = updatedReservation.Topic;
        reservation.Date = updatedReservation.Date;
        reservation.StartTime = updatedReservation.StartTime;
        reservation.EndTime = updatedReservation.EndTime;
        reservation.Status = updatedReservation.Status;
        
        return Ok(reservation);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = InMemoryDB.Reservations.FirstOrDefault(r => r.Id == id);
        
        if (reservation is null) return NotFound();
        
        InMemoryDB.Reservations.Remove(reservation);
        
        return NoContent();
    }
    
    private static bool HasTimeConflict(Reservation reservation, int? ignoredReservationId = null)
    {
        return InMemoryDB.Reservations.Any(existing =>
            existing.Id != ignoredReservationId &&
            existing.RoomId == reservation.RoomId &&
            existing.Date == reservation.Date &&
            reservation.StartTime < existing.EndTime &&
            reservation.EndTime > existing.StartTime
        );
    }
    
}