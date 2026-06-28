using FlightScan.Core.Enums;

namespace FlightScan.Core.Specifications
{
    public class FlightSpecParams : BaseSpecParams
    {
        public bool? IsCancelled { get; set; }
        public City? WhereFrom { get; set; }
        public City? WhereTo { get; set; }
        public bool? HasAvailableSeats { get; set; }
        public bool? NonstopOnly { get; set; }
    }
}
