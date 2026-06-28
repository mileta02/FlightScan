namespace FlightScan.Core.Responses.Flights
{
    public class FlightResponse
    {
        public int Id { get; set; }
        public string WhereFrom { get; set; } = null!;
        public string WhereTo { get; set; } = null!;
        public int Stops { get; set; }
        public DateTime DepartureDate { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsLowAvailability { get; set; }
    }
}
