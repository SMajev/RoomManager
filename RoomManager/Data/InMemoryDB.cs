using RoomManager.Models;

namespace RoomManager.Data;

public class InMemoryDB
{
    public static List<Room> Rooms { get; } = new()
    {
        new Room
        {
            Id = 1, Name = "Sala 101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 2, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = true
        },
        new Room
        {
            Id = 3, Name = "Sala 305", BuildingCode = "A", Floor = 3, Capacity = 35, HasProjector = false,
            IsActive = true
        },
        new Room
        {
            Id = 4, Name = "Audytorium", BuildingCode = "C", Floor = 0, Capacity = 80, HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 5, Name = "Sala archiwalna", BuildingCode = "B", Floor = 1, Capacity = 12, HasProjector = false,
            IsActive = false
        }
    };
        
    public static List<Reservation> Reservations { get; } = new()
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Nowak",
            Topic = "Podstawy C#",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(11, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Anna Kowalska",
            Topic = "Warsztaty z HTTP i REST",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 30),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Piotr Zieliński",
            Topic = "Konsultacje projektowe",
            Date = new DateOnly(2026, 5, 11),
            StartTime = new TimeOnly(13, 0),
            EndTime = new TimeOnly(15, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 4,
            OrganizerName = "Maria Wiśniewska",
            Topic = "Szkolenie zespołowe",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(8, 30),
            EndTime = new TimeOnly(12, 0),
            Status = "confirmed"
        }
    };
}