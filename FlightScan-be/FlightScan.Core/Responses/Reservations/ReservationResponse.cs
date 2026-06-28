namespace FlightScan.Core.Responses.Reservations
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string WhereFrom { get; set; } = null!;
        public string WhereTo { get; set; } = null!;
        public DateTime DepartureDate { get; set; }
        public int ReservedSeats { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
