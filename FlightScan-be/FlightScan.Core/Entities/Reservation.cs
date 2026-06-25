using FlightScan.Core.Enums;

namespace FlightScan.Core.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public Flight Flight { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ReservedSeats { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
