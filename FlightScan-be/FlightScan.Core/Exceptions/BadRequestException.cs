namespace FlightScan.Core.Exceptions
{
    public class BadRequestException : GlobalException
    {
        public BadRequestException(string message) : base("400", message) { }
    }
}
