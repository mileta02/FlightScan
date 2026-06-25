namespace FlightScan.Core.Exceptions
{
    public class NotFoundException : GlobalException
    {
        public NotFoundException(string message) : base("404", message) { }
    }
}
