namespace FlightScan.Core.Exceptions
{
    public class UnauthorizedException : GlobalException
    {
        public UnauthorizedException(string message) : base("401", message) { }
    }
}
